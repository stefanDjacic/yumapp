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
        //private static readonly List<string> adminIds = new();

        public async Task AddNewNotificationsBE(string userId)
        {
            await Clients.User(userId).SendAsync("AddNewNotificationsFE", userId);
        }

        public async Task AddCommentToPostBE(CommentModel commentModel)
        {
            await Clients.All.SendAsync("AddCommentToPostFE", commentModel);
        }

        //public async Task AddNewGroupNotificationsBE(string groupName)
        //{
        //    await Clients.Group(groupName).SendAsync("AddNewGroupNotificationsFE"/*, adminIds*/); //change this, will probably need new table, maybe its better to add user to admin group via onconnected hub method
        //}

        //public async Task AddToGroup(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //    adminIds.Add(Context.User.FindFirst(ClaimTypes.NameIdentifier).ToString());

        //    Context.User.IsInRole("admin"); //use this
        //}

        //public async override Task OnDisconnectedAsync(Exception exception)
        //{
        //    //adminIds.Remove(Context.User.FindFirst(ClaimTypes.NameIdentifier).ToString());
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin");
        //    await base.OnDisconnectedAsync(exception);
        //}

        //public async override Task OnConnectedAsync()
        //{
        //    if (Context.User.IsInRole("admin"))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
        //    }

        //    await base.OnConnectedAsync();
        //}
    }
}
