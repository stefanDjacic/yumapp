using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.ViewsComponent
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(bool isSignedIn)
        {
            if (isSignedIn)
            {
                return View("NavbarSignedInViewComponent1");
            }
            else
            {
                return Content(string.Empty);
            }
        }
    }
}
