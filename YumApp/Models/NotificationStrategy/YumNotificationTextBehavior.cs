using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models.NotificationStrategy
{
    public class YumNotificationTextBehavior : INotificationTextStrategy
    {
        public string GetNotificationText(string initiatorName)
        {
            return $"{ initiatorName } finds your post yummy!";
        }
    }
}
