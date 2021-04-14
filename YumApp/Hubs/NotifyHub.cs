using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Hubs
{
    public class NotifyHub : Hub
    {
        public async Task NotifyUser()
        {
            IGroupManager group = n
            
            await this.Clients.User("2").SendAsync()
        }
    }
}
