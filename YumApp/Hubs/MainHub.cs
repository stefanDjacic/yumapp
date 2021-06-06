using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Hubs
{
    //Calls functions from frontend
    public class MainHub : Hub
    {     
        public async Task AddNewNotificationsBE(string userId)
        {
            await Clients.User(userId).SendAsync("AddNewNotificationsFE", userId);
        }

        public async Task AddCommentToPostBE(CommentModel commentModel)
        {
            await Clients.All.SendAsync("AddCommentToPostFE", commentModel);
        }
    }
}
