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
        public async Task FollowNotification(AppUserModel receiver, AppUserModel initator)
        {
            await Clients.User(receiver.Id.ToString()).SendAsync("receiveNotification", initator, $"{ initator.FirstName } { initator.LastName } has followed you!");
        }

        //public async Task Notifyuser(NotificationModel notificationmodel)
        //{
        //    await Clients.User(notificationmodel.Receiver.Id.ToString()).SendAsync("receiveNotification", notificationmodel.Initiator, notificationmodel.NotificationText);
        //}

        //public async Task NotifyUser(string receiverId, string initiatorId)
        //{
        //    await Clients.User(receiverId).SendAsync("receiveNotification",)
        //}
    }
}
