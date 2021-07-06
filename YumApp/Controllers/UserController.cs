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
        private readonly ICRDRepository<Ingredient> _ingredientRepository;
        private readonly ICRDRepository<Post_Ingredient> _post_IngredientRepository;
        private readonly IHttpClientFactory _httpClientFactory;        
        private readonly string _userPhotoFolderPath;

        public UserController(AppUserManager appUserManager,
                              ICRUDRepository<Post> postRepository,
                              ICRDRepository<User_Follows> user_FollowsRepository,
                              ICRDRepository<Notification> notificationRepository,
                              ICRDRepository<Yummy_Post> yummy_PostRepository,
                              ICRDRepository<Comment> commentRepository,
                              ICRDRepository<Ingredient> ingredientRepository,
                              ICRDRepository<Post_Ingredient> post_IngredientRepository,
                              IHttpClientFactory httpClientFactory,                              
                              IWebHostEnvironment webHostEnvironment)
        {            
            _appUserManager = appUserManager;
            _postRepository = postRepository;            
            _user_FollowsRepository = user_FollowsRepository;
            _notificationRepository = notificationRepository;
            _yummy_PostRepository = yummy_PostRepository;
            _commentRepository = commentRepository;
            _ingredientRepository = ingredientRepository;
            _post_IngredientRepository = post_IngredientRepository;
            _httpClientFactory = httpClientFactory;            
            _userPhotoFolderPath = webHostEnvironment.ContentRootPath + @"\wwwroot\Photos\UserPhotos\";
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            //Gets currently logged in user
            AppUser currentUser = await _appUserManager.GetUserAsync(User);

            //Returns List<Post> of user whose profile is being viewed, as split query,
            //because of cartesian explosion
            List<Post> userPosts = await _postRepository.GetAll()
                                                        .Where(p => p.AppUserId == id)
                                                        .Include(p => p.Post_Ingredients)
                                                        .ThenInclude(pi => pi.Ingredient)
                                                        .Include(p => p.AppUser)
                                                        .Include(p => p.Comments)
                                                        .ThenInclude(c => c.Commentator)
                                                        .OrderByDescending(p => p.TimeOfPosting)
                                                        .AsSplitQuery()
                                                        .AsNoTracking()
                                                        .ToListAsync();
            //Convers Posts to PostsModel type
            List<PostModel> userPostsModel = userPosts.ToPostModel()
                                                      .ToList();

            #region Most efficient query for userPostsModel, use it when entity framework core 6.0 gets released!! AsSplitQuery() doesn't work on projections yet!!
            //Most efficient query for postmodel, use it when entity framework core 6.0 gets released!! Don't forget to chain AsSplitQuery()!!
            //var userPostsModel = _postRepository.GetAll()
            //                                    .Where(p => p.AppUserId == id)
            //                                    .ToPostModelTest()
            //                                    .ToList();
            #endregion

            //Gets the user whose profile is being viewed
            AppUserModel userModelProfile = await _appUserManager.FindByIdAsync(id.ToString())
                                                          .ContinueWith(au => au.Result.ToAppUserModelBaseInfo());
            //Checks and sets bool if current user is following the one whose profile is being viewed
            userModelProfile.IsBeingFollowed = _user_FollowsRepository.GetAll()
                                                               .Any(u => u.FollowerId == currentUser.Id && u.FollowsId == id);

            //Gets all the posts which current user has liked from user whose profile he is visiting
            List<Yummy_Post> currentUsersYummedPosts = await _yummy_PostRepository.GetAll()
                                                                                  .Where(yp => yp.AppUserId == currentUser.Id && yp.PostAppUserId == id)
                                                                                  .ToListAsync();
            //Sets IsPostYummed property if current user has already liked the post, would probably be the best to denormalize database instead of doing this
            foreach (var yummedPost in currentUsersYummedPosts)
            {
                userPostsModel.SingleOrDefault(p => p.Id == yummedPost.PostId).IsPostYummed = true;
            }

            //To pass necessary data for view
            ViewBag.UserProfile = userModelProfile;
            ViewBag.CurrentUser = currentUser;

            return View(userPostsModel);
        }

        [HttpGet]
        public async Task<IActionResult> Feed()
        {
            //Gets currently logged in user
            AppUser currentUser = await _appUserManager.GetUserAsync(User);

            //Returns List<Post> of users who are being followed by current user, as split query,
            //because of cartesian explosion
            List<Post> posts = await _postRepository.GetAll()
                                                    .Where(p => p.AppUser.Follow.Any(uf => uf.FollowerId == currentUser.Id))
                                                    .Include(p => p.AppUser)
                                                    .Include(p => p.Post_Ingredients)
                                                    .ThenInclude(pi => pi.Ingredient)
                                                    .Include(p => p.Comments)
                                                    .ThenInclude(c => c.Commentator)
                                                    .OrderByDescending(p => p.TimeOfPosting)
                                                    .AsSplitQuery()
                                                    .AsNoTracking()
                                                    .ToListAsync();
            //Convers Posts to PostsModel type
            List<PostModel> postsModel = posts.ToPostModel().ToList();

            //Gets all the posts which current user has liked and which belong to users who are being followed by current user
            //You can like someones post without following him
            List<Yummy_Post> currentUsersYummedPostsOfFollowedUsers = await _yummy_PostRepository.GetAll()
                                                                                  .Where(yp => yp.AppUserId == currentUser.Id &&
                                                                                               yp.Post.AppUser.Follow.Any(uf => uf.FollowerId == currentUser.Id))                                                                                  
                                                                                  .ToListAsync();
            //Sets IsPostYummed property if current user has already liked the post
            foreach (var yummedPost in currentUsersYummedPostsOfFollowedUsers)
            {
                postsModel.SingleOrDefault(p => p.Id == yummedPost.PostId).IsPostYummed = true;
            }

            //To pass necessary data for view
            ViewBag.CurrentUser = currentUser;            

            return View(postsModel);
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
                //In case API call fails, notify the user
                ModelState.AddModelError("Country", "Problem with loading countries, please try again later.");
            }

            //Gets currently logged in user as AppUserModel type
            AppUserModel currentUser = await _appUserManager.GetUserAsync(User)
                                                            .ContinueWith(u => u.Result.ToAppUserModelBaseInfo());

            //To pass necessary data for view
            ViewBag.CurrentUser = currentUser.ToAppUserEntity();
            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(AppUserModel model)
        {
            try
            {
                //Checks ModelState
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //Checks if user changed profile photo, if so, it changes photo path from the model
                if (model.Photo != null)
                {
                    model.PhotoPath = ControllerHelperMethods.UpdatePhotoPath(_userPhotoFolderPath, model.Photo.FileName, model.Id);
                }
                
                //Converts AppUserModel to entity type, so it can be updated
                AppUser user = model.ToAppUserEntity();                
                IdentityResult result = await _appUserManager.UpdateUserAsync(user);
                
                //Checks if update has succeeded
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                //Deletes old photo from server, saves new photo, if no photos were supplied, returns
                await ControllerHelperMethods.SavePhoto(model.Photo, _userPhotoFolderPath, model.Id );

                return RedirectToAction("Profile", "User", new { id = model.Id });
            }
            catch (Exception ex)
            {
                //In case of exception, returns the View with model and errors
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SinglePost(int id)
        {
            //Gets the post
            Post post = _postRepository.GetAll()
                                       .Include(p => p.Post_Ingredients)
                                       .ThenInclude(pi => pi.Ingredient)
                                       .Include(p => p.AppUser)
                                       .Include(p => p.Comments)
                                       .ThenInclude(c => c.Commentator)
                                       .Where(p => p.Id == id)
                                       .AsSplitQuery()
                                       .AsNoTracking()
                                       .SingleOrDefault(p => p.Id == id);
            //In case the post has been deleted
            if (post == null)
            {
                return View("NotFound");
            }

            //Converts to PostModel
            PostModel postModel = post.ToPostModel();

            //Gets currently logged in user
            AppUser currentUser = await _appUserManager.GetUserAsync(User);
            //Changes bool of post depending if current user has already liked the post
            postModel.IsPostYummed = _yummy_PostRepository.GetAll()
                                                          .Any(yp => yp.PostId == id && yp.AppUserId == currentUser.Id);

            //Need a List<PostModel> type for partial view
            List<PostModel> newPostModel = new() { postModel };

            //To pass necessary data for view
            ViewBag.CurrentUser = currentUser;

            return View(newPostModel);
        }

        [HttpPost]
        public async Task FollowUser(int id)
        {
            //Gets current user and the one who is followed
            AppUser currentUser = await _appUserManager.GetUserAsync(User);
            AppUser followedUser = await _appUserManager.FindByIdAsync(id.ToString());

            //So time matches in different tables
            DateTime currentDateTime = DateTime.Now;

            //Creates new instance for following
            User_Follows userFollows = new ()
            {
                FollowerId = currentUser.Id,
                FollowsId = id,
                DateOfFollowing = currentDateTime
            };
            //Adds new row to database
            await _user_FollowsRepository.Add(userFollows);
            
            //Creates new NotificationModel instance (to take advantage of strategy pattern) and converts it to Notification entity
            Notification newNotification = new NotificationModel(currentUser.FirstName,
                                                                 currentUser.LastName, 
                                                                 currentDateTime,
                                                                 currentUser.Id, 
                                                                 new FollowNotificationTextBehavoir())
                                                                .ToNotificationEntity(currentUser.Id, followedUser.Id);
            //Adds new notification to the database
            await _notificationRepository.Add(newNotification);

            return;
        }

        [HttpPost]
        public async Task UnfollowUser(int id)
        {
            //Gets Id of currently logged in user
            int currentUserId = await _appUserManager.GetCurrentUserIdAsync(User);

            //Gets the row of current user following the specified one
            User_Follows userFollows = await _user_FollowsRepository.GetAll()
                                                                    .SingleOrDefaultAsync(uf => uf.FollowerId == currentUserId && uf.FollowsId == id);
            //Deletes the row
            await _user_FollowsRepository.Remove(userFollows);
            
            return;
        }

        [HttpPost]
        public async Task<IActionResult> YumAPost(int id)
        {
            //Gets the current user
            AppUser currentUser = await _appUserManager.GetUserAsync(User);
            //Gets the post which current user liked
            Post yummedPost = await _postRepository.GetSingle(id);

            //Increments the number of likes and updates the instance
            yummedPost.NumberOfYums++;
            await _postRepository.Update(yummedPost);

            //So time matches in different tables
            var currentDateTime = DateTime.Now;

            //Liked post instance
            Yummy_Post yummy_Post = new()
            {
                AppUserId = currentUser.Id,
                DateYummed = currentDateTime,
                PostId = yummedPost.Id,
                PostAppUserId = yummedPost.AppUserId
            };
            //Adding liked post to database
            await _yummy_PostRepository.Add(yummy_Post);

            //Creates new NotificationModel instance (to take advantage of strategy pattern) and converts it to Notification entity
            Notification newNotification = new NotificationModel(currentUser.FirstName,
                                                                 currentUser.LastName,
                                                                 currentDateTime,
                                                                 yummedPost.Id, 
                                                                 new YumNotificationTextBehavior())
                                                                .ToNotificationEntity(currentUser.Id, yummedPost.AppUserId);
            //Adds new notification to the database
            await _notificationRepository.Add(newNotification);

            //Returns the incremented number of likes
            return Json(yummedPost.NumberOfYums);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(int id)
        {
            //Gets notifications of specified user
            List<NotificationModel> notifications = await _notificationRepository.GetAll()
                                                             .Where(n => n.ReceiverId == id)                                                             
                                                             .ToNotificationModel()
                                                             .OrderByDescending(n => n.TimeOfNotification)
                                                             .ToListAsync();
            
            return Json(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> PostAComment(int id, string commentText) 
        {
            //Gets the current user
            AppUser currentUser = await _appUserManager.GetUserAsync(User);
            //Gets specified post
            Post commentedPost = await _postRepository.GetSingle(id);

            //So time matches in different tables
            var currentDateTime = DateTime.Now;

            //Creates new CommentModel instance
            var newCommentModel = new CommentModel
            {
                Commentator = currentUser.ToAppUserModelBaseInfo(),
                Content = commentText,
                TimeOfCommenting = currentDateTime,
                AppUserId = commentedPost.AppUserId,
                PostId = id
            };
            //Adds Comment entity to database
            await _commentRepository.Add(newCommentModel.ToCommentEntity());

            //Creates new NotificationModel instance (to take advantage of strategy pattern) and converts it to Notification entity
            Notification newNotification = new NotificationModel(currentUser.FirstName,
                                                                 currentUser.LastName,
                                                                 currentDateTime,
                                                                 id,
                                                                 new CommentNotificationTextBehavior())
                                                                 .ToNotificationEntity(currentUser.Id, commentedPost.AppUserId);
            //Adds new notification to the database
            await _notificationRepository.Add(newNotification);

            return Json(newCommentModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetIngredients()
        {
            //Gets all ingredients from database
            var ingredients = await _ingredientRepository.GetAll()
                                                         .ToIngredientModel()
                                                         .OrderBy(i => i.Name)
                                                         .ToListAsync();

            return Json(ingredients);
        }

        [HttpPost]
        public async Task<IActionResult> PostAPost(string postContent, string postNotes, string[] postIngredientIds)
        {
            //Gets the current user
            AppUser currentUser = await _appUserManager.GetUserAsync(User);

            //Checks if Comment.Content is null
            if (string.IsNullOrWhiteSpace(postContent))
            {
                ModelState.AddModelError("Content", "Content is required.");  
                return Json(new { redirectToUrl = Url.Action("Profile", "User", new { id = currentUser.Id }) });
            }            

            //Creates new Post instance
            Post newPost = new()
            {
                Content = postContent,
                Notes = postNotes,
                AppUserId = currentUser.Id,
                NumberOfYums = 0,
                TimeOfPosting = DateTime.Now
            };
            //Adds newPost instance to database
            Post addedPost = await _postRepository.Add(newPost);            

            //Creates and adds post_Ingredient instances to database
            //Very bad performace due to constantly SaveChanges() for each object, should AddRange() instead
            foreach (var ingredientId in postIngredientIds)
            {
                var post_Ingredient = new Post_Ingredient
                {
                    PostId = addedPost.Id,
                    AppUserId = addedPost.AppUserId,
                    IngredientId = int.Parse(ingredientId)
                };

                await _post_IngredientRepository.Add(post_Ingredient);
            }

            return Json(new { redirectToUrl = Url.Action("SinglePost", "User", new { id = addedPost.Id }) });
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowingUsers(int id)
        {
            //Get all users that are followed by specified user
            List<AppUserModel> followingUsers = await _user_FollowsRepository.GetAll()
                                                                             .Where(uf => uf.Follower.Id == id)
                                                                             .Select(uf => uf.Follows)
                                                                             .ToAppUserModelBaseInfo()
                                                                             .ToListAsync();
            return Json(followingUsers);
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowers(int id)
        {
            //Gets all followers of specified user
            List<AppUserModel> followers = await _user_FollowsRepository.GetAll()
                                                                        .Where(uf => uf.Follows.Id == id)
                                                                        .Select(uf => uf.Follower)
                                                                        .ToAppUserModelBaseInfo()
                                                                        .ToListAsync();
            return Json(followers);
        }

        [HttpGet]
        public async Task<IActionResult> GetYummedPosts(int id)
        {
            //Gets pots liked by specified user
            List<Post> yummedPosts = await _postRepository.GetAll()
                                                    .Where(p => p.Yummy_Posts.Any(yp => yp.AppUserId == id))
                                                    .Include(p => p.AppUser)
                                                    .Include(p => p.Post_Ingredients)
                                                    .ThenInclude(pi => pi.Ingredient)
                                                    .OrderByDescending(p => p.TimeOfPosting)
                                                    .AsSplitQuery()
                                                    .AsNoTracking()
                                                    .ToListAsync();
            List<PostModel> yummedPostsModel = yummedPosts.ToPostModel().ToList();

            return Json(yummedPostsModel);
        }

        [HttpPost]
        public async Task ReportAPost(int id)
        {
            //Gets a post
            Post reportedPost = await _postRepository.GetSingle(id);
            //Checks if post has already been reported
            if (!reportedPost.IsReported)
            {
                //Marks it as reported
                reportedPost.IsReported = true;
                //Updates the change
                await _postRepository.Update(reportedPost);
            }

            return;
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string userName)
        {
            //Gets users as userModel types
            List<AppUserModel> usersModel = await _appUserManager.Users
                                                       .Where(au => EF.Functions.Like(au.FirstName.ToUpper() + " " + au.LastName.ToUpper(),
                                                                                      $"%{userName.Trim().ToUpper()}%"))
                                                       .ToAppUserModelBaseInfo()
                                                       .ToListAsync();
            return View(usersModel);
        }
    }
}
