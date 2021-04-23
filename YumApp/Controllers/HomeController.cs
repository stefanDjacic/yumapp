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
        //private readonly UserManager<AppUser> _userManager;
        private readonly AppUserManager _appUserManager;
        private readonly SignInManager<AppUser> _signInManager;        

        public HomeController(/*UserManager<AppUser> userManager,*/
                              AppUserManager appUserManager,
                              SignInManager<AppUser> signInManager)
        {
            //_userManager = userManager;
            _appUserManager = appUserManager;
            _signInManager = signInManager;            
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {                
                int currentUserId = await _appUserManager.GetCurrentUserIdAsync(User);

                return RedirectToAction("Profile", "User", new { id = currentUserId });                 //OVO MENJAJ
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                AppUser newUser = model.ToAppUserEntity();
                //var result = await _appUserManager.CreateAsync(newUser, model.Password);
                var result = await _appUserManager.CreateAsync(newUser, newUser.PasswordHash);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("Index", model);
                }

                await _appUserManager.AddToRoleAsync(newUser, "normaluser");                
                await _signInManager.SignInAsync(newUser, false);
                
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
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        //int currentUserId = await ControllerHelperMethods.GetCurrentUserIdAsync(_userManager, model.Email);
                        int currentUserId = await _appUserManager.GetCurrentUserIdAsync(model.Email);

                        return RedirectToAction("Profile", "User", new { id = currentUserId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
