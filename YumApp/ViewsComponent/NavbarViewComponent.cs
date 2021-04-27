using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Controllers.HelperAndExtensionMethods;
using YumApp.Models;

namespace YumApp.ViewsComponent
{
    public class NavbarViewComponent : ViewComponent
    {        
        private readonly AppUserManager _appUserManager;

        public NavbarViewComponent(AppUserManager appUserManager)
        {            
            _appUserManager = appUserManager;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            if (!isSignedIn)
            {
                return Content(string.Empty);
            }

            var currentUser = _appUserManager.FindByNameAsync(User.Identity.Name).Result;
            //AppUser currentUser = ViewBag.CurrentUser;
            //int currentUserId = int.Parse(HttpContext.Request.Cookies["MyCookie"]);
            var currentUserModel = _appUserManager.GetUserWithNotificationsById(currentUser.Id);

            return View("NavbarSignedInViewComponent1", currentUserModel);
        }
    }
}
