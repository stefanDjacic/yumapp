using EntityLibrary.Repository;
using EntityLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using YumApp.Models.NotificationStrategy;
using Microsoft.AspNetCore.SignalR;
using YumApp.Hubs;
using YumApp.Controllers.HelperAndExtensionMethods;

namespace YumApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {        
        private readonly AppUserManager _appUserManager;
        private readonly ICRUDRepository<Post> _postRepository;
        private readonly ICRDRepository<User_Follows> _user_FollowsRepository;
        private readonly ICRDRepository<Notification> _notificationRepository;
        private readonly ICRDRepository<Comment> _commentRepo;
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IHubContext<NotifyHub> _hubContext;
        private readonly string _userPhotoFolderPath;

        public UserController(AppUserManager appUserManager,
                              ICRUDRepository<Post> postRepository,
                              ICRDRepository<User_Follows> user_FollowsRepository,
                              ICRDRepository<Notification> notificationRepository,
                              ICRDRepository<Comment> commentRepo,//obrisi ovo
                              IHttpClientFactory httpClientFactory,
                              //IHubContext<NotifyHub> hubContext,
                              IWebHostEnvironment webHostEnvironment)
        {            
            _appUserManager = appUserManager;
            _postRepository = postRepository;            
            _user_FollowsRepository = user_FollowsRepository;
            _notificationRepository = notificationRepository;
            _commentRepo = commentRepo;
            _httpClientFactory = httpClientFactory;
            //_hubContext = hubContext;
            _userPhotoFolderPath = webHostEnvironment.ContentRootPath + @"\wwwroot\Photos\UserPhotos\";
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {

            var comments = _commentRepo.GetAll().Where(c => c.AppUserId == id).ToCommentModelTest().ToList();
            //delete this line after testing
            var postModelTest = _postRepository.GetAll().Where(p => p.AppUserId == id).ToPostModelTest().ToList();

            //Gets currently logged in user from database
            AppUser currentUser = await _appUserManager.GetUserAsync(User);

            //Returns List<Post> and loads List<PostModel> of user whose profile is being viewed, from database with split query,
            //because of cartesian explosion
            List<Post> userPosts = await _postRepository.GetAll()
                                                        .Include(p => p.Post_Ingredients)
                                                        .ThenInclude(pi => pi.Ingredient)
                                                        .Include(p => p.AppUser)
                                                        .Include(p => p.Comments)
                                                        .ThenInclude(c => c.Commentator)
                                                        .Where(p => p.AppUserId == id)
                                                        .AsSplitQuery()
                                                        .AsNoTracking()
                                                        .ToListAsync();

            List<PostModel> userPostsModel = userPosts.ToPostModel()
                                                      .ToList();

            //Gets the user (post owner) from List<PostModel>
            AppUserModel userModel = userPostsModel.Select(p => p.User).FirstOrDefault();

            //Checks and sets bool if current user is following the one whose profile is being viewed, from database
            userModel.IsBeingFollowed = _user_FollowsRepository.GetAll()
                                                               .Any(u => u.FollowerId == currentUser.Id && u.FollowsId == id);

            ViewBag.UserProfile = userModel;
            ViewBag.CurrentUser = currentUser;

            return View(userPostsModel);

            #region BadQuery
            ////Returns List<Post> and loads List<PostModel> of user whose profile is being viewed, from database using small queries instead of one big,
            ////because of cartesian explosion
            //List<PostModel> userPostsModel = await _postRepository.GetAll()
            //                                             .Include(p => p.Post_Ingredients)
            //                                             .ThenInclude(pi => pi.)
            //                                             .Include(p => p.Comments)
            //                                             .ThenInclude(c => c.Commentator)
            //                                             .Where(p => p.AppUserId == id)
            //                                             .ToPostModel()
            //                                             .ToListAsync();
            #endregion

            #region TestingQueries
            /*
             TESTING QUERY EFFICIENCY
            */
            //var userPosts = await _postRepository.GetAll().Select(p => new PostTest
            //{
            //    Id = p.Id,
            //    Content = p.Content,
            //    Name = p.AppUser.FirstName,
            //    Comments = p.Comments.Select(c => new TestComment
            //    {
            //        Content = c.Content,
            //        TestAppUser = new TestAppUser { Id = c.Commentator.Id, Name = c.Commentator.FirstName }
            //    }).ToList()
            //}).ToListAsync();

            //var userPosts = await _postRepository.GetAll().ToPostTest1().ToListAsync();

            //return View("TestView", userPosts);
            #endregion
        }

        [HttpGet]
        public IActionResult Feed()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            HttpResponseMessage response = await ControllerHelperMethods.CallApi(HttpMethod.Get, "https://restcountries.eu/rest/v2/all", _httpClientFactory);

            if (response.IsSuccessStatusCode)
            {
                //Getting countrie from API and storing them in ViewBag to display them as select list in AppUserModel
                Country[] countries = await response.Content.ReadFromJsonAsync<Country[]>();
                ViewBag.Countries = new SelectList(countries, "Name", "Name", "Name");                
            }
            else
            {
                ModelState.AddModelError("Country", "Problem with loading countries, please try again later.");
            }

            var currentUser = await _appUserManager.GetUserAsync(User)
                                                   .ContinueWith(u => u.Result.ToAppUserModelBaseInfo());            

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(AppUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.Photo != null)
                {
                    model.PhotoPath = ControllerHelperMethods.UpdatePhotoPath(_userPhotoFolderPath, model.Photo.FileName, model.Id);
                }
                
                AppUser user = model.ToAppUserEntity();
                
                IdentityResult result = await _appUserManager.UpdateUserAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                await ControllerHelperMethods.SavePhoto(model.Photo, _userPhotoFolderPath, model.Id );

                return RedirectToAction("Profile", "User", new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        [HttpPost]
        public async Task FollowUser(int id)
        {
            var currentUser = await _appUserManager.GetUserAsync(User);
            var followedUser = await _appUserManager.FindByIdAsync(id.ToString());

            var userFollows = new User_Follows
            {
                FollowerId = currentUser.Id,
                FollowsId = id,
                DateOfFollowing = DateTime.UtcNow
            };

            await _user_FollowsRepository.Add(userFollows);
            
            Notification newNotification = new NotificationModel(currentUser.FirstName, currentUser.LastName, new FollowNotificationTextBehavoir())
                                                       .ToNotificationEntity(currentUser.Id, followedUser.Id);

            await _notificationRepository.Add(newNotification);

            return;
        }

        [HttpPost]
        public async Task UnfollowUser(int id)
        {
            int currentUserId = await _appUserManager.GetCurrentUserIdAsync(User); /*int.Parse(Request.Cookies["MyCookie"]);*/

            var userFollows = await _user_FollowsRepository.GetAll()
                                                           .SingleOrDefaultAsync(uf => uf.FollowerId == currentUserId && uf.FollowsId == id);

            await _user_FollowsRepository.Remove(userFollows);
            
            return;
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
