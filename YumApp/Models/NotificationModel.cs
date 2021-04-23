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
        //public static IQueryable<NotificationModel> ToNotificationModel(this IQueryable<Notification> entities)
        //{
        //    return entities.Select(n => new NotificationModel
        //                                    {
        //                                        Receiver = n.Receiver.ToAppUserModel(),
        //                                        //Initiator = n.Initiator.ToAppUserModel(),
        //                                        NotificationText = n.NotificationText
        //                                    }
        //    );
        //}

        public static IEnumerable<NotificationModel> ToNotificationModel(this IEnumerable<Notification> entities)
        {
            return entities.Select(n => new NotificationModel
            {
                Receiver = new AppUserModel
                {
                    Id = n.Receiver.Id,
                    FirstName = n.Receiver.FirstName,
                    LastName = n.Receiver.LastName,
                    Email = n.Receiver.Email,
                    Username = n.Receiver.UserName,
                    About = n.Receiver.About,
                    DateCreated = n.Receiver.DateCreated,
                    DateOfBirth = n.Receiver.DateOfBirth,
                    Gender = n.Receiver.Gender,
                    PhotoPath = n.Receiver.PhotoPath,
                    Country = n.Receiver.Country
                },
                Initiator = new AppUserModel
                {
                    Id = n.Initiator.Id,
                    FirstName = n.Initiator.FirstName,
                    LastName = n.Initiator.LastName,
                    Email = n.Initiator.Email,
                    Username = n.Initiator.UserName,
                    About = n.Initiator.About,
                    DateCreated = n.Initiator.DateCreated,
                    DateOfBirth = n.Initiator.DateOfBirth,
                    Gender = n.Initiator.Gender,
                    PhotoPath = n.Initiator.PhotoPath,
                    Country = n.Initiator.Country
                },
                NotificationText = n.NotificationText
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
            //Initiator = initiator;
            Receiver = receiver;
            NotificationText = notificationTextStrategy.GetNotificationText(initiator.FirstName + " " + initiator.LastName);
        }

        //public INotificationTextStrategy NotificationTextStrategy { get; set; }
        public string NotificationText { get; set; }
        public AppUserModel Receiver { get; set; }
        public AppUserModel Initiator { get; set; }
    }
}
