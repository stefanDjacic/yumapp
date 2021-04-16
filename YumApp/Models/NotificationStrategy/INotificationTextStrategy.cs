using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models.NotificationStrategy
{
    public interface INotificationTextStrategy
    {
        string GetNotificationText(string initiatorName);
    }
}
