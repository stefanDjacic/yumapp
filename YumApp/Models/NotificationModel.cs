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
        public static IEnumerable<NotificationModel> ToNotificationModel(this IEnumerable<Notification> notifications)
        {
            return notifications.Select(n => new NotificationModel
            {
                Receiver = n.Receiver.ToAppUserModelBaseInfo(),
                Initiator = n.Initiator.ToAppUserModelBaseInfo(),
                NotificationText = n.NotificationText
            });
        }

        public static IEnumerable<NotificationModel> ToNotificationModelTEST(this IEnumerable<Notification> notifications)
        {
            return notifications.Select(n => new NotificationModel
            {
                //Receiver = n.Receiver.ToAppUserModelBaseInfo(),
                InitiatorFullName = n.Initiator.FirstName + " " + n.Initiator.LastName,
                InitiatorPhotoPath = n.Initiator.PhotoPath,
                NotificationText = n.NotificationText
            });
        }
        #region bad code
        //public static IEnumerable<NotificationModel> ToNotificationModel(this IEnumerable<Notification> entities)
        //{
        //    return entities.Select(n => new NotificationModel
        //    {
        //        Receiver = new AppUserModel
        //                        {
        //                            Id = n.Receiver.Id,
        //                            FirstName = n.Receiver.FirstName,
        //                            LastName = n.Receiver.LastName,
        //                            Email = n.Receiver.Email,
        //                            Username = n.Receiver.UserName,
        //                            About = n.Receiver.About,                                    
        //                            DateOfBirth = n.Receiver.DateOfBirth,
        //                            Gender = n.Receiver.Gender,
        //                            PhotoPath = n.Receiver.PhotoPath,
        //                            Country = n.Receiver.Country
        //                        },
        //        Initiator = new AppUserModel
        //                        {
        //                            Id = n.Initiator.Id,
        //                            FirstName = n.Initiator.FirstName,
        //                            LastName = n.Initiator.LastName,
        //                            Email = n.Initiator.Email,
        //                            Username = n.Initiator.UserName,
        //                            About = n.Initiator.About,                                    
        //                            DateOfBirth = n.Initiator.DateOfBirth,
        //                            Gender = n.Initiator.Gender,
        //                            PhotoPath = n.Initiator.PhotoPath,
        //                            Country = n.Initiator.Country
        //                        },
        //        NotificationText = n.NotificationText
        //    }
        //    );
        //}
        #endregion
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


        //testing
        public string InitiatorPhotoPath { get; set; }
        public string InitiatorFullName { get; set; }
    }
}
