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
                TimeOfNotification = notification.TimeOfNotification,
                IdForRedirecting = notification.IdForRedirecting
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
                TimeOfNotification = notification.TimeOfNotification,
                IdForRedirecting = notification.IdForRedirecting
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
    }

    public class NotificationModel
    {
        public NotificationModel()
        {
        }
        public NotificationModel(string initiatorFirstName, string initiatorLastName, DateTime timeOfNotification, int idForRedirecting, INotificationTextStrategy notificationTextStrategy)
        {
            InitiatorFullName = initiatorFirstName + " " + initiatorLastName;
            TimeOfNotification = timeOfNotification;
            IdForRedirecting = idForRedirecting;
            NotificationText = notificationTextStrategy.GetNotificationText(InitiatorFullName);
        }

        public string NotificationText { get; set; }

        public DateTime TimeOfNotification { get; set; }

        public string InitiatorPhotoPath { get; set; }

        public string InitiatorFullName { get; set; }

        public int IdForRedirecting { get; set; }
    }
}
