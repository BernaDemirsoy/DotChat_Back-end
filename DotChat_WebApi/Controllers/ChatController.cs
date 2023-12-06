using AutoMapper;
using DotChat_DTOs.Chat;
using DotChat_Entities.DbSet;
using DotChat_Repositories.Abstract;
using DotChat_Repositories.Concrete.Hubs;
using DotChat_Repositories.Context;
using DotChat_Services.Abstact;
using DotChat_Services.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotChat_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
  
        private readonly UserManager<User> userManager;
        private readonly IHubContext<MessageHub,IMessageClient> hubContext;
        private readonly IGenericService<ChatConnectionLog> chatConnectionService;
        private readonly IMapper mapper;
        private readonly AppDbContext appDbContext;

        public ChatController(UserManager<User> userManager,IHubContext<MessageHub, IMessageClient> hubContext,IGenericService<ChatConnectionLog> chatConnectionService, IMapper mapper)
        {
            
            this.userManager = userManager;
            this.hubContext = hubContext;
            this.chatConnectionService = chatConnectionService;
            this.mapper = mapper;
        }

        [HttpGet("{message}")]
        //[Route("[action]")]
        public async Task<IActionResult> SendMessageAsync(string message)
        {
            await hubContext.Clients.All.receiveMessage(message);
            return Ok();
        }

        [HttpPost]
        //[Route("[action]")]
        public async Task<IActionResult> ConnectingLog(ConnectedDto connectedDto)
        {
            User user = connectedDto.user;
            userManager.UpdateAsync(user);
            if (user != null && !string.IsNullOrEmpty(connectedDto.connectionId))
            {
                ChatConnectionLog chatConnectionLog = new ChatConnectionLog
                {
                    user = user,
                    userId = user.Id,
                    connectionId=connectedDto.connectionId,
                    connectedDate = DateTime.UtcNow,
                };
                chatConnectionService.Add(chatConnectionLog);

                return Ok("Log kaydedildi");
            }

            return BadRequest("Sistemde bir client yok veya connectionId düzegün alınamadı");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllContacts(string userid)
        {
            var userList = userManager.Users.ToList();
            userList.Remove(userList.Where(x=>x.Id==userid).ToList().FirstOrDefault());
            List<ContactUsersDto> contactUserList = new List<ContactUsersDto>();
            if (userList.Count > 0)
            {
                mapper.Map(userList, contactUserList);
                return Ok(contactUserList);
            }
            else
                return BadRequest();

        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                User user = await userManager.FindByIdAsync(id);
                if (!string.IsNullOrEmpty(user.Id))
                {
                    return Ok(user);
                }
                else
                    return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
           

        }
    }
}
