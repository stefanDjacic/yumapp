using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Hubs
{
    public class NotifyHub : Hub
    {
        public async Task NotifyUser(NotificationModel notificationModel)
        {
            await Clients.User(notificationModel.Receiver.Id.ToString()).SendAsync("ReceiveNotification", notificationModel.Initiator, notificationModel.NotificationText);
        }
    }
}
