using AutoMapper;
using AutoMapper.Execution;
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
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace DotChat_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly IHubContext<MessageHub, IMessageClient> hubContext;
        private readonly IGenericService<ChatConnectionLog> chatConnectionService;
        private readonly IGenericService<ChatGroup> chatgroupService;
        private readonly IGenericService<ChatGroupMember> chatGroupMemberService;
        private readonly IMapper mapper;
  

        public ChatController(UserManager<User> userManager, IHubContext<MessageHub, IMessageClient> hubContext, IGenericService<ChatConnectionLog> chatConnectionService, IGenericService<ChatGroup> chatgroupService, IGenericService<ChatGroupMember> chatGroupMemberService, IMapper mapper)
        {

            this.userManager = userManager;
            this.hubContext = hubContext;
            this.chatConnectionService = chatConnectionService;
            this.chatgroupService = chatgroupService;
            this.chatGroupMemberService = chatGroupMemberService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SendMessageAsync(SendMessageDto sendMessageDto)
        {
            try
            {
                var group = await chatgroupService.GetByIdAsync(sendMessageDto.groupId);
                await hubContext.Clients.Group(group.description).receiveMessage(sendMessageDto.message);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> createGroup(CreateGroupDto createGroupDto)
        {
            //Not:birden fazla aynı descriptionlı ve aynı üyeli grup yaratılabilir mi?? Bu casede yaratıyor
            try
            {
                ChatGroup chatGroup = new ChatGroup();
                mapper.Map(createGroupDto, chatGroup);
                var allChatGroups = await chatgroupService.GetAllAsync();
                if (allChatGroups.Select(a => a.description).Contains(chatGroup.description))
                {
                    return BadRequest("This Group has already created");
                }
                bool result=await chatgroupService.AddAsync(chatGroup);

                if (result)
                {
                    await hubContext.Groups.AddToGroupAsync(createGroupDto.connectionId, createGroupDto.description);
                    var allGroups = chatgroupService.GetAllAsync();
                    await hubContext.Clients.All.groups(allChatGroups);
                    return Ok(chatGroup);
                }
                else
                {
                    return BadRequest("Group not added");
                }
            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> createGroupMembers(GroupMembersDto groupMembersDto)
        {

            try
            {
                if (groupMembersDto.chatGroup != null)
                {
                    List<ChatGroupMember> members = new List<ChatGroupMember>();
                    var chatgroup = await chatgroupService.GetByIdAsync(groupMembersDto.chatGroup.Id);
                    for (int i = 0; i < groupMembersDto.users.Count(); i++)
                    {
                        User userDto = userManager.Users.Where(a => a.Id == groupMembersDto.users[i].Id).FirstOrDefault();

                        ChatGroupMember chatGroupMember = new ChatGroupMember
                        {
                            chatGroup = chatgroup,
                            chatgroupId = chatgroup.Id,
                            memberUserId = userDto.Id,
                            user = userDto,
                        };

                        members.Add(chatGroupMember);
                    }
                    bool result = await chatGroupMemberService.AddAsync(members);
                    if (!result)
                    {
                        return BadRequest($"Ekleme işlemi gerçekleştirilirken bir hata ile karşılaşıldı={result}");
                    }
                    chatgroup.chatGroupMembers = members;
                    await chatgroupService.UpdateAsync(chatgroup);
                    var Allresult = new
                    {
                        members = members,
                        result = result
                    };
                    return Ok(Allresult);
                }
                return BadRequest("There is no groups that you can start conversition");
            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }
            

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllGroupChats(string userid)
        {
            try
            {
                //var chatGroupMembersAll = await chatGroupMemberService.GetAllAsync(a => a.chatGroup);
                //var chatGroupMember= chatGroupMembersAll.Where(a=>a.memberUserId== userid).Select(a=>a.chatGroup).ToList();
                //if (chatGroupMembers.Count() <= 0)
                //{
                //    return BadRequest($"Not found any started conversation related to the user has {userid} userId");
                //}
                //List<ChatGroup> chatGroups = new List<ChatGroup>();
                //foreach (var member in chatGroupMember)
                //{
                //    var chatgroup = await chatgroupService.GetByIdAsync(member.chatgroupId);
                //    chatGroups.Add(chatgroup);
                //}
                var chatGroups= await chatgroupService.GetAllAsync();
                return Ok(chatGroups);

            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }


        }

        [HttpPost]
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
                    connectionId = connectedDto.connectionId,
                    connectedDate = DateTime.UtcNow,
                };
               await chatConnectionService.AddAsync(chatConnectionLog);

                return Ok("Log kaydedildi");
            }

            return BadRequest("Sistemde bir client yok veya connectionId düzegün alınamadı");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllContacts(string userid)
        {
            try
            {
                var userList = userManager.Users.ToList();
                userList.Remove(userList.Where(x => x.Id == userid).ToList().FirstOrDefault());
                List<ContactUsersDto> contactUserList = new List<ContactUsersDto>();
                if (userList.Count > 0)
                {
                    mapper.Map(userList, contactUserList);
                    return Ok(contactUserList);
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest($"{ex.Message}");
            }
            

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
            catch (Exception ex)
            {

                return BadRequest($"{ex.Message}");
            }


        }
    }
}
