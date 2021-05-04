using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YumApp.Models.NotificationStrategy;

namespace YumApp.Models
{
    public static class NotificationModelExtensionMethods
    {
        //Expression for IQueryable, because it has problems with projections
        public static readonly Expression<Func<Notification, NotificationModel>> MapNotificationToNotificationModel = 
            notification => new NotificationModel
            {
                InitiatorFullName = notification.Initiator.FirstName + " " + notification.Initiator.LastName,
                InitiatorPhotoPath = notification.Initiator.PhotoPath,
                NotificationText = notification.NotificationText,
                TimeOfNotification = notification.TimeOfNotification
            };

        public static IQueryable<NotificationModel> ToNotificationModel(this IQueryable<Notification> notifications)
        {
            return notifications.Select(MapNotificationToNotificationModel);
        }

        public static Notification ToNotificationEntity(this NotificationModel notification, int initiatorId, int receiverId)
        {
            return new Notification
            {
                InitiatorId = initiatorId,
                ReceiverId = receiverId,
                NotificationText = notification.NotificationText,
                TimeOfNotification = notification.TimeOfNotification
            };
        }

        #region Doesn't work because of IQueryable
        //private static NotificationModel CreateNewNotificationModel(this Notification notification)
        //{
        //    return new NotificationModel
        //    {
        //        InitiatorFullName = notification.Initiator.FirstName + " " + notification.Initiator.LastName,
        //        InitiatorPhotoPath = notification.Initiator.PhotoPath,
        //        NotificationText = notification.NotificationText,
        //        TimeOfNotification = notification.TimeOfNotification
        //    };
        //}

        //public static IEnumerable<NotificationModel> ToNotificationModel(this IEnumerable<Notification> notifications)
        //{
        //    return notifications.Select(n => n.CreateNewNotificationModel());
        //}
        #endregion

        #region Bad code
        //public static IEnumerable<NotificationModel> ToNotificationModel(this IEnumerable<Notification> notifications)
        //{
        //    return notifications.Select(n => new NotificationModel
        //    {
        //        InitiatorFullName = n.Initiator.FirstName + " " + n.Initiator.LastName,
        //        InitiatorPhotoPath = n.Initiator.PhotoPath,
        //        NotificationText = n.NotificationText,
        //        TimeOfNotification = n.TimeOfNotification
        //    });
        //}

        //public static IQueryable<NotificationModel> ToNotificationModel(this IQueryable<Notification> notifications)
        //{
        //    return notifications.Select(n => new NotificationModel
        //    {
        //        InitiatorFullName = n.Initiator.FirstName + " " + n.Initiator.LastName,
        //        InitiatorPhotoPath = n.Initiator.PhotoPath,
        //        NotificationText = n.NotificationText,
        //        TimeOfNotification = n.TimeOfNotification
        //    });
        //}
        #endregion

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
        public NotificationModel(string initiatorFirstName, string initiatorLastName, INotificationTextStrategy notificationTextStrategy)
        {
            InitiatorFullName = initiatorFirstName + " " + initiatorLastName;
            NotificationText = notificationTextStrategy.GetNotificationText(InitiatorFullName);
        }

        public string NotificationText { get; set; }

        public DateTime TimeOfNotification { get; set; }

        public string InitiatorPhotoPath { get; set; }

        public string InitiatorFullName { get; set; }
    }
}
