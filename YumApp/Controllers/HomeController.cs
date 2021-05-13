using EntityLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Controllers.HelperAndExtensionMethods;
using YumApp.Models;

namespace YumApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {        
        private readonly AppUserManager _appUserManager;
        private readonly SignInManager<AppUser> _signInManager;        

        public HomeController(AppUserManager appUserManager,
                              SignInManager<AppUser> signInManager)
        {
            _appUserManager = appUserManager;
            _signInManager = signInManager;            
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Checkes if current user is logged in to redirect him to his profile
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = await _appUserManager.GetCurrentUserIdAsync(User);

                return RedirectToAction("Profile", "User", new { id = currentUserId });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                //Checks if model is valid
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                
                //Converts RegisterModel to AppUser type
                AppUser newUser = model.ToAppUserEntity();
                //Sets default properties for new user
                newUser.SetDefaultPhotoPathAndUserName();

                //Creates new user
                var result = await _appUserManager.CreateAsync(newUser, newUser.PasswordHash);

                //Checks if user is created and returns model with errors if it didn't
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("Index", model);
                }
                
                //Adds new user to role
                await _appUserManager.AddToRoleAsync(newUser, "normaluser");     
                
                //Signs in new user
                await _signInManager.SignInAsync(newUser, false);
                
                //Gets id of new user to redirect him to his profile
                int currentUserId = await _appUserManager.GetCurrentUserIdAsync(newUser.Email);

                return RedirectToAction("Profile", "User", new { id = currentUserId });
            }
            catch
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {         
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                //Checks if modelState is valid
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //Tries to sign in user
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                //Checks if signing in failed
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

                //Gets current user's id
                int currentUserId = await _appUserManager.GetCurrentUserIdAsync(model.Email);


                //Creates new cookie with currently logged in user id
                this.CreateUserIdCookie(currentUserId, "/User", "MyCookie");

                return RedirectToAction("Profile", "User", new { id = currentUserId });
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Signs out the user
            await _signInManager.SignOutAsync();

            //Deletes user id cookie
            this.RemoveUserIdCookie();

            return RedirectToAction("Index", "Home");
        }
    }
}
