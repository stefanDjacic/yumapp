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

namespace YumApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICRUDRepository<Post> _postRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _userPhotoFolderPath;

        public UserController(UserManager<AppUser> userManager, ICRUDRepository<Post> postRepository, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _httpClientFactory = httpClientFactory;
            _userPhotoFolderPath = webHostEnvironment.ContentRootPath + @"\wwwroot\Photos\UserPhotos\";
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            //Returns user whose profile is being viewed
            var user = await _userManager.FindByIdAsync(id.ToString());
            ViewBag.UserProfile = user.ToAppUserModel();
            
            //Returns PostModel of selected user
            var userPosts = await _postRepository.GetAll()
                                                .Where(p => p.AppUserId == id)
                                                .ToPostModel()
                                                .ToListAsync();

            return View(userPosts);

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
            var response = await ControllerHelperMethods.CallApi(HttpMethod.Get, "https://restcountries.eu/rest/v2/all", _httpClientFactory);

            if (response.IsSuccessStatusCode)
            {
                //Getting countrie from API and storing them in ViewBag to display them as select list in AppUserModel
                var countries = await response.Content.ReadFromJsonAsync<Country[]>();
                ViewBag.Countries = new SelectList(countries, "Name", "Name", "Name");
            }
            else
            {
                ModelState.AddModelError("Country", "Problem with loading countries, please try again later.");
            }

            var currentUser = await _userManager.CurrentUserToAppUserModel(User);
            //var currentUser = await _userManager.FindByIdAsync(id.ToString());

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

                model.PhotoPath = ControllerHelperMethods.UpdatePhotoPath(_userPhotoFolderPath, model.Photo.FileName, model.Id);
                var appuser = model.ToAppUserEntity();
                
                var result = await _userManager.UpdateUserAsync(appuser);

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Settings(AppUserModel model)
        //{
        //    try
        //    {                
        //        if (ModelState.IsValid)
        //        {
        //            var appuser = model.ToAppUserEntity();

        //            var result = await _userManager.UpdateUserAsync(appuser);

        //            if (result.Succeeded)
        //            {                        
        //                return RedirectToAction("Profile", "User", new { id = model.Id });
        //            }
        //            else
        //            {
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }

        //                return View(model);
        //            }
        //        }
        //        else
        //        {
        //            return View(model);
        //        }
        //    }
        //    catch
        //    {
        //        return View(model);                
        //    }
        //}

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
