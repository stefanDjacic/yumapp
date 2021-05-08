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
        private readonly ICRDRepository<Yummy_Post> _yummy_PostRepository;
        private readonly ICRDRepository<Comment> _commentRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IHubContext<NotifyHub> _hubContext;
        private readonly string _userPhotoFolderPath;

        public UserController(AppUserManager appUserManager,
                              ICRUDRepository<Post> postRepository,
                              ICRDRepository<User_Follows> user_FollowsRepository,
                              ICRDRepository<Notification> notificationRepository,
                              ICRDRepository<Yummy_Post> yummy_PostRepository,
                              ICRDRepository<Comment> commentRepository,
                              IHttpClientFactory httpClientFactory,
                              //IHubContext<NotifyHub> hubContext,
                              IWebHostEnvironment webHostEnvironment)
        {            
            _appUserManager = appUserManager;
            _postRepository = postRepository;            
            _user_FollowsRepository = user_FollowsRepository;
            _notificationRepository = notificationRepository;
            _yummy_PostRepository = yummy_PostRepository;
            _commentRepository = commentRepository;
            _httpClientFactory = httpClientFactory;
            //_hubContext = hubContext;
            _userPhotoFolderPath = webHostEnvironment.ContentRootPath + @"\wwwroot\Photos\UserPhotos\";
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            //delete this line after testing
            //var comments = _commentRepo.GetAll().Where(c => c.AppUserId == id).ToCommentModelTest().ToList();

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

            #region Most efficient query for userPostsModel, use it when entity framework core 6.0 gets released!! AsSplitQuery() doesn't work on projections yet!!
            //Most efficient query for postmodel, use it when entity framework core 6.0 gets released!! Don't forget to chain AsSplitQuery()!!
            //var userPostsModel = _postRepository.GetAll()
            //                                    .Where(p => p.AppUserId == id)
            //                                    .ToPostModelTest()
            //                                    .ToList();
            #endregion

            //Gets the user (post owner) from List<PostModel>
            AppUserModel userModel = userPostsModel.Select(p => p.User).FirstOrDefault();
            //Checks and sets bool if current user is following the one whose profile is being viewed, from database
            userModel.IsBeingFollowed = _user_FollowsRepository.GetAll()
                                                               .Any(u => u.FollowerId == currentUser.Id && u.FollowsId == id);

            //Gets all the posts which current user liked from user whose profile he is visiting
            var currentUsersYummedPosts = await _yummy_PostRepository.GetAll()
                                                                     .Where(yp => yp.AppUserId == currentUser.Id && yp.PostAppUserId == id)
                                                                     .ToListAsync();
            //Sets IsPostYummed property if current user has already liked the post
            foreach (var yummedPost in currentUsersYummedPosts)
            {
                userPostsModel.SingleOrDefault(p => p.Id == yummedPost.PostId).IsPostYummed = true;
            }

            ViewBag.UserProfile = userModel;
            ViewBag.CurrentUser = currentUser;

            return View(userPostsModel);
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

        [HttpGet]
        public async Task<IActionResult> SinglePost(int id)
        {
            var post = _postRepository.GetAll()
                                            .Include(p => p.Post_Ingredients)
                                            .ThenInclude(pi => pi.Ingredient)
                                            .Include(p => p.AppUser)
                                            .Include(p => p.Comments)
                                            .ThenInclude(c => c.Commentator)
                                            //.Where(p => p.Id == id)
                                            .AsSplitQuery()
                                            .AsNoTracking()
                                            .SingleOrDefault(p => p.Id == id);
                                            //.ToListAsync();

            var postModel = post.ToPostModel();

            var currentUser = await _appUserManager.GetUserAsync(User);

            postModel.IsPostYummed = _yummy_PostRepository.GetAll()
                                                          .Any(yp => yp.PostId == id && yp.AppUserId == currentUser.Id);

            //Have to create new list, because PostPartial view expects one
            List<PostModel> newPostModel = new() { postModel };

            ViewBag.CurrentUser = currentUser;

            return View(newPostModel);
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
            
            Notification newNotification = new NotificationModel(currentUser.FirstName,
                                                                 currentUser.LastName, 
                                                                 DateTime.Now,
                                                                 currentUser.Id, 
                                                                 new FollowNotificationTextBehavoir())
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

        [HttpPost]
        public async Task<IActionResult> YumAPost(int id)
        {
            var currentUser = await _appUserManager.GetUserAsync(User);
            var yummedPost = await _postRepository.GetSingle(id);


            yummedPost.NumberOfYums++;
            await _postRepository.Update(yummedPost);

            Yummy_Post yummy_Post = new()
            {
                AppUserId = currentUser.Id,
                DateYummed = DateTime.Now,
                PostId = yummedPost.Id,
                PostAppUserId = yummedPost.AppUserId
            };
            await _yummy_PostRepository.Add(yummy_Post);

            Notification newNotification = new NotificationModel(currentUser.FirstName,
                                                                 currentUser.LastName,
                                                                 DateTime.Now,
                                                                 yummedPost.Id, 
                                                                 new YumNotificationTextBehavior())
                                                                .ToNotificationEntity(currentUser.Id, yummedPost.AppUserId);
            await _notificationRepository.Add(newNotification);

            return Json(yummedPost.NumberOfYums);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(int id)
        {
            var notifications = await _notificationRepository.GetAll()
                                                             .Where(n => n.ReceiverId == id)                                                             
                                                             .ToNotificationModel()
                                                             .OrderByDescending(n => n.TimeOfNotification)
                                                             .ToListAsync();

            return Json(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> PostAComment() 
        {

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
