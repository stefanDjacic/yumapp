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

namespace YumApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICRUDRepository<Post> _postRepository;
        private readonly IHttpClientFactory _httpClient;

        public UserController(UserManager<AppUser> userManager, ICRUDRepository<Post> postRepository, IHttpClientFactory httpClient)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            //Returns user whose profile is being viewed
            var user = await _userManager.FindByIdAsync(id.ToString());
            ViewBag.UserProfile = user.ToAppUserModel();

            //var currentUserId = await ControllerHelperMethods.GetCurrentUserIdAsync(_userManager, User);
            var UserPosts = await _postRepository.GetAll()
                                                .Where(p => p.AppUserId == id)
                                                .ToPostModel()
                                                .ToListAsync();

            return View(UserPosts);
        }
        
        [HttpGet]
        public IActionResult Feed()
        {
            return View();
        }

        [HttpGet]        
        public async Task<IActionResult> Settings(int id)
        {            
            var client = _httpClient.CreateClient();
            try
            {
                var countriesModel = await client.GetFromJsonAsync<CountriesModel>("https://restcountries.eu/rest/v2/all");
                countriesModel.Countries.Insert(0, new Country { Name = string.Empty});                
                ViewBag.Countries = new SelectList(countriesModel.Countries, "Name");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);                
            }

            var currentUser = await _userManager.FindByIdAsync(id.ToString());

            return View(/*currentUser.ToAppUserModel()*/ );
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
