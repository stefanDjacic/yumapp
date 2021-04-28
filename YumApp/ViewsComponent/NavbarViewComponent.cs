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
        private readonly AppUserManager _appUserManager;
        private readonly ICRDRepository<Notification> _notificationRepository;

        public NavbarViewComponent(AppUserManager appUserManager, ICRDRepository<Notification> notificationRepository)
        {            
            _appUserManager = appUserManager;
            _notificationRepository = notificationRepository;
        }

        public IViewComponentResult Invoke(bool isSignedIn)
        {
            if (!isSignedIn)
            {
                return Content(string.Empty);
            }

            var currentUser = _appUserManager.FindByNameAsync(User.Identity.Name).Result;
            ////AppUser currentUser = ViewBag.CurrentUser;
            ////int currentUserId = int.Parse(HttpContext.Request.Cookies["MyCookie"]);
            //var currentUserModel = _appUserManager.GetUserWithNotificationsById(currentUser.Id);


            //Gets notifications of current user with necessary data
            List<NotificationModel> notificationsModel = _notificationRepository.GetAll()
                                                           .Where(n => n.ReceiverId == currentUser.Id)
                                                           .ToNotificationModelTEST()
                                                           .ToList();

            ViewBag.CurrentUserId = currentUser.Id;

            return View("NavbarSignedInViewComponent1", /*currentUserModel*/notificationsModel);
        }
    }
}
