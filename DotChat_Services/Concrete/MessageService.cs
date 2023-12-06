using DotChat_Entities.DbSet;
using DotChat_Repositories.Abstract;
using DotChat_Repositories.Concrete.Hubs;
using DotChat_Repositories.Context;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Services.Concrete
{
    public class MessageService
    {
        private readonly IHubContext<MessageHub,IMessageClient> hubContext;
        private readonly AppDbContext dbContext;

        public MessageService(IHubContext<MessageHub, IMessageClient> hubContext,AppDbContext dbContext)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContext;
        }

        public async Task SendMessageAsync(string message)
        {
             await hubContext.Clients.All.receiveMessage(message);
            
        }

      
    }
}
