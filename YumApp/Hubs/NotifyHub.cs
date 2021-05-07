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
        public async Task AddNewNotificationsBE(int userId)
        {
            await Clients.User(userId.ToString()).SendAsync("AddNewNotificationsFE");
        }

        //public async Task FollowNotification(AppUserModel receiver, AppUserModel initator)
        //{
        //    await Clients.User(receiver.Id.ToString()).SendAsync("receiveNotification", initator, $"{ initator.FirstName } { initator.LastName } has followed you!");
        //}        
    }
}
