using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Controllers.HelperAndExtensionMethods
{
    public class AppUserStore : UserStore<AppUser, IdentityRole<int>, YumAppDbContext, int>
    {
        public AppUserStore(YumAppDbContext context,
                            IdentityErrorDescriber identityErrorDescriber = null)
                            : base(context, identityErrorDescriber)
        {
        }

        public AppUserModel GetUserWithNotificationsById(int id)
        {
            //automatski
            var appUserModelWithNotifications = Context.AppUsers
                                                       .Include(au => au.NotificationsReceiver)
                                                       .ThenInclude(n => n.Initiator)
                                                       .Where(au => au.Id == id)
                                                       .ToAppUserModel()
                                                       .AsNoTracking()
                                                       .SingleOrDefault();

            #region slower query
            //manuelno
            //var appUserModelWithNotifications = Context.AppUsers.Where(au => au.Id == id).Select(appUser => new AppUserModel
            //{
            //    Id = appUser.Id,
            //    FirstName = appUser.FirstName,
            //    LastName = appUser.LastName,
            //    Email = appUser.Email,
            //    Username = appUser.UserName,
            //    DateOfBirth = appUser.DateOfBirth,
            //    Country = appUser.Country,
            //    Gender = appUser.Gender,
            //    About = appUser.About,
            //    PhotoPath = appUser.PhotoPath,
            //    Notifications = appUser.NotificationsReceiver.Select(n => new NotificationModel
            //    {
            //        Initiator = new AppUserModel
            //        {
            //            Id = n.Initiator.Id,
            //            FirstName = n.Initiator.FirstName,
            //            LastName = n.Initiator.LastName,
            //            Email = n.Initiator.Email,
            //            Username = n.Initiator.UserName,
            //            DateOfBirth = n.Initiator.DateOfBirth,
            //            Country = n.Initiator.Country,
            //            Gender = n.Initiator.Gender,
            //            About = n.Initiator.About,
            //            PhotoPath = n.Initiator.PhotoPath
            //        },
            //        Receiver = new AppUserModel
            //        {
            //            Id = appUser.Id,
            //            FirstName = n.Receiver.FirstName,
            //            LastName = n.Receiver.LastName,
            //            Email = n.Receiver.Email,
            //            Username = n.Receiver.UserName,
            //            DateOfBirth = n.Receiver.DateOfBirth,
            //            Country = n.Receiver.Country,
            //            Gender = n.Receiver.Gender,
            //            About = n.Receiver.About,
            //            PhotoPath = n.Receiver.PhotoPath
            //        },
            //        NotificationText = n.NotificationText
            //    }).ToList()
            //}).SingleOrDefault();
            #endregion

            return appUserModelWithNotifications;
        }
    }
}

