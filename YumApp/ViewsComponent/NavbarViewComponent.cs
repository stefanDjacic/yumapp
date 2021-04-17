using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.ViewsComponent
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public NavbarViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            var currentUser = _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User).Result;

            if (isSignedIn)
            {
                return View("NavbarSignedInViewComponent1", currentUser);
            }
            else
            {
                return Content(string.Empty);
            }
        }
    }
}
