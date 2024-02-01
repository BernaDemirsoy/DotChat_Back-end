using DotChat_DTOs.Chat;
using DotChat_Entities.DbSet;
using DotChat_Repositories.Abstract;
using DotChat_Repositories.Context;
using DotChat_Services.Abstact;
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
        private readonly UserManager<User> userManager;
        private readonly IGenericService<ChatConnectionLog> chatConnectionService;
        private readonly IGenericService<ChatGroupMessages> chatGroupMessagesService;
        static bool isConnected=false;
        public MessageHub(UserManager<User> userManager, IGenericService<ChatConnectionLog> chatConnectionService,IGenericService<ChatGroupMessages> chatGroupMessagesService)
        {
            this.userManager = userManager;
            this.chatConnectionService = chatConnectionService;
            this.chatGroupMessagesService = chatGroupMessagesService;
        }

        
        public async Task GetUser(User user)
        {
            //user connection id güncelleyecek
           
            User updatedUser=userManager.Users.Where(a => a.Id == user.Id).FirstOrDefault();
            updatedUser.connectionId = Context.ConnectionId;
            await userManager.UpdateAsync(updatedUser);

            var connectionLog = new ChatConnectionLog
            {
                user = updatedUser,
                userId = updatedUser.Id,
                connectionId = Context.ConnectionId,
                connectedDate = isConnected ? DateTime.UtcNow : (DateTime?)null, // Bağlandığında tarih, koparsa null
                disConnectedDate = !isConnected ? DateTime.UtcNow : (DateTime?)null // Koparsa tarih, bağlandığında null
            };

            bool result=await chatConnectionService.AddAsync(connectionLog);

        }

        public async Task SendMessageToBinaryGroup(SendMessageDto sendMessageDto) 
        {
            string clientConnectionId = userManager.Users.Where(a => a.Id == sendMessageDto.receiverClientId).FirstOrDefault().connectionId;
           
             await Clients.Clients(clientConnectionId, Context.ConnectionId).receiveMessage(sendMessageDto.message);
            
            
        }
        public async Task SetUnreadedMessage(UnreadedMessagesCountDto messagesCountDto)
        {
            string clientConnectionId = userManager.Users.Where(a => a.Id == messagesCountDto.receiverUserId).FirstOrDefault().connectionId;
            string name = userManager.Users.Where(a => a.Id == messagesCountDto.currentUserId).FirstOrDefault().UserName;
            var allMessages = await chatGroupMessagesService.GetAllAsync();
            var filteredMessagesCount = allMessages.Where(a => a.ChatGroupId == messagesCountDto.groupChatId && a.isRead == false).Count();
            UnreadCountHubDtocs result = new UnreadCountHubDtocs
            {
                count = filteredMessagesCount,
                userName = name,
            };

            await Clients.Client(clientConnectionId).receiveCount(result);
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            isConnected = true;
            await Clients.All.UserJoin(Context.ConnectionId);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //ConnectionId nulla çekilsin connectionId üzerinden gidilecek


            User user = userManager.Users.FirstOrDefault(a => Context.ConnectionId.Contains(a.connectionId)==true);
            if (user != null)
            {
                user.connectionId = null;
                await userManager.UpdateAsync(user);
            }
            isConnected = false;

            await base.OnDisconnectedAsync(exception);


        }

    }
}
