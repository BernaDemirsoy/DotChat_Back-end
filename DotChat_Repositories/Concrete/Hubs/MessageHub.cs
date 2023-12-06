using DotChat_Entities.DbSet;
using DotChat_Repositories.Abstract;
using DotChat_Repositories.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Repositories.Concrete.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        //static List<string> clients = new List<string>();

        public async Task GetConnectionId()
        {
            await Clients.All.receiveConnectionId(Context.ConnectionId);
        }

        public override async Task OnConnectedAsync()
        {
            //Userın connectedIdsi Fronta gönderilir frontta tekrar chat controllerdaki apiye bağlanılarak connection log oluşturulur.

            await Clients.All.UserJoin(Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //Userın connectedIdsi Fronta gönderilir frontta tekrar chat controllerdaki apiye bağlanılarak disconnection logu oluşturulur.
            await Clients.All.UserLeaved(Context.ConnectionId);
        }

        public async Task SendMessageAsync(string message)
        {
            await Clients.All.receiveMessage(message);
        }
    }
}
