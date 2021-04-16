using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models.NotificationStrategy;

namespace YumApp.Models
{
    public class NotificationModel
    {
        public NotificationModel(AppUserModel initiator, AppUserModel receiver, INotificationTextStrategy notificationTextStrategy)
        {
            Initiator = initiator;
            Receiver = receiver;
            NotificationText = notificationTextStrategy.GetNotificationText(initiator.FirstName + " " + initiator.LastName);
        }

        //public INotificationTextStrategy NotificationTextStrategy { get; set; }
        public string NotificationText { get; private set; }
        public AppUserModel Receiver { get; set; }
        public AppUserModel Initiator { get; set; }
    }
}
