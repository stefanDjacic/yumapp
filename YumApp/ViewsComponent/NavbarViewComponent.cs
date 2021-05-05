using EntityLibrary;
using EntityLibrary.Repository;
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
        //private readonly AppUserManager _appUserManager;
        private readonly ICRDRepository<Notification> _notificationRepository;

        public NavbarViewComponent(/*AppUserManager appUserManager,*/ ICRDRepository<Notification> notificationRepository)
        {            
            //_appUserManager = appUserManager;
            _notificationRepository = notificationRepository;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            if (!isSignedIn)
            {
                return Content(string.Empty);
            }

            //var currentUser = _appUserManager.FindByNameAsync(User.Identity.Name).Result;

            //Gets id of currently logged in user from cookie
            int currentUserId = int.Parse(Request.Cookies["MyCookie"]);

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
