using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models.NotificationStrategy
{
    public class PostReportTextBehavior : INotificationTextStrategy
    {
        public string GetNotificationText(string initiatorName)
        {
            return $"{ initiatorName } has reported a post.";
        }
    }
}
