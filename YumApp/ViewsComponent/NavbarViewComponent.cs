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
        private readonly ICRDRepository<Notification> _notificationRepository;

        public NavbarViewComponent(ICRDRepository<Notification> notificationRepository)
        {                        
            _notificationRepository = notificationRepository;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            if (!isSignedIn)
            {
                return Content(string.Empty);
            }

            //Gets id of currently logged in user from cookie, if it can't parse it, admin is the logged in user and is on ~/Admin path
            int currentUserId;
            if (!int.TryParse(Request.Cookies["MyCookie"], out currentUserId))
            {
                currentUserId = 1;
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
