using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using Microsoft.AspNetCore.Cors;

namespace DashBoardWebAPI.Hubs
{
    public class MyHub : Hub
    {
        public  async Task SendMessages()
        {
            
            await Clients.All.SendAsync("updateMessages");
        }

    }
}
