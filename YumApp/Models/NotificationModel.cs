using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models.NotificationStrategy;

namespace YumApp.Models
{
    public static class NotificationModelExtensionMethods
    {
        public static IQueryable<NotificationModel> ToNotificationModel(this IQueryable<Notification> entities)
        {
            return entities.Select(n => new NotificationModel
                                            {
                                                Receiver = n.AppUser.ToAppUserModel(),
                                                Initiator = n.Doer.ToAppUserModel(),
                                                NotificationText = n.NotificationText
                                            }
            );
        }

        public static IEnumerable<NotificationModel> ToNotificationModel(this IEnumerable<Notification> entities)
        {
            return entities.Select(n => new NotificationModel
            {
                Receiver = n.AppUser.ToAppUserModel(),
                Initiator = n.Doer.ToAppUserModel(),
                NotificationText = n.NotificationText
                //Receiver = new AppUserModel
                //{
                //    Id = n.AppUser.Id,
                //    FirstName = n.AppUser.FirstName,
                //    LastName = n.AppUser.LastName,
                //    Email = n.AppUser.Email,
                //    Username = n.AppUser.UserName,
                //    About = n.AppUser.About,
                //    DateCreated = n.AppUser.DateCreated,
                //    DateOfBirth = n.AppUser.DateOfBirth,
                //    Gender = n.AppUser.Gender,
                //    PhotoPath = n.AppUser.PhotoPath,
                //    Country = n.AppUser.Country
                //},
                //Initiator = new AppUserModel
                //{
                //    Id = n.Doer.Id,
                //    FirstName = n.Doer.FirstName,
                //    LastName = n.Doer.LastName,
                //    Email = n.Doer.Email,
                //    Username = n.Doer.UserName,
                //    About = n.Doer.About,
                //    DateCreated = n.Doer.DateCreated,
                //    DateOfBirth = n.Doer.DateOfBirth,
                //    Gender = n.Doer.Gender,
                //    PhotoPath = n.Doer.PhotoPath,
                //    Country = n.Doer.Country
                //},
                //NotificationText = n.NotificationText
            }
            );
        }
    }

    public class NotificationModel
    {
        public NotificationModel()
        {
        }
        public NotificationModel(AppUserModel initiator, AppUserModel receiver, INotificationTextStrategy notificationTextStrategy)
        {
            Initiator = initiator;
            Receiver = receiver;
            NotificationText = notificationTextStrategy.GetNotificationText(initiator.FirstName + " " + initiator.LastName);
        }

        //public INotificationTextStrategy NotificationTextStrategy { get; set; }
        public string NotificationText { get; set; }
        public AppUserModel Receiver { get; set; }
        public AppUserModel Initiator { get; set; }
    }
}
