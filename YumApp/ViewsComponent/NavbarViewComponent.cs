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
        private readonly UserManager<AppUser> _userManager;
        private readonly AppUserManager _appUserManager;

        public NavbarViewComponent(UserManager<AppUser> userManager, AppUserManager appUserManager)
        {
            _userManager = userManager;
            _appUserManager = appUserManager;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            //var currentUser = _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User).Result; //ovo promeni i dodaj mu notifikacije

            var currentUserModel = _appUserManager.GetUserWithNotifications((ClaimsPrincipal)User);

            if (isSignedIn)
            {
                return View("NavbarSignedInViewComponent1", currentUserModel);
            }
            else
            {
                return Content(string.Empty);
            }
        }
    }
}
