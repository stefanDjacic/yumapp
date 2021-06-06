using EntityLibrary;
using EntityLibrary.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Controllers;
using YumApp.Controllers.HelperAndExtensionMethods;
using YumApp.Models;

namespace YumApp.ViewsComponent
{
    public class NavbarViewComponent : ViewComponent
    {        
        private readonly ICRDRepository<Notification> _notificationRepository;
        private readonly AppUserManager _appUserManager;

        public NavbarViewComponent(ICRDRepository<Notification> notificationRepository, AppUserManager appUserManager)
        {                        
            _notificationRepository = notificationRepository;
            _appUserManager = appUserManager;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            //Checks if user is signed in to add/remove the navbar
            if (!isSignedIn)
            {
                return Content(string.Empty);
            }

            //Gets id of currently logged in user from cookie, if it can't parse it, gets id from database
            //Because ["MyCookie"] is only for /User urls and not /Admin
            int currentUserId;
            if (!int.TryParse(Request.Cookies["MyCookie"], out currentUserId))
            {
                currentUserId = _appUserManager.GetCurrentUserIdAsync((ClaimsPrincipal)User).Result;
            }

            //Gets notifications of current user
            List<NotificationModel> notificationsModel = _notificationRepository.GetAll()
                                                                                .Where(n => n.ReceiverId == currentUserId)
                                                                                .ToNotificationModel()
                                                                                .OrderByDescending(n => n.TimeOfNotification)
                                                                                .ToList();

            ViewBag.CurrentUserId = currentUserId;

            return View("NavbarSignedInViewComponent1", notificationsModel);
        }
    }
}
