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
        private readonly IGenericService<ChatGroupMessages> chatGroupMessageService;
        private readonly IMapper mapper;
  

        public ChatController(UserManager<User> userManager, IHubContext<MessageHub, IMessageClient> hubContext, IGenericService<ChatConnectionLog> chatConnectionService, IGenericService<ChatGroup> chatgroupService, IGenericService<ChatGroupMember> chatGroupMemberService, IGenericService<ChatGroupMessages> chatGroupMessageService,IMapper mapper)
        {

            this.userManager = userManager;
            this.hubContext = hubContext;
            this.chatConnectionService = chatConnectionService;
            this.chatgroupService = chatgroupService;
            this.chatGroupMemberService = chatGroupMemberService;
            this.chatGroupMessageService = chatGroupMessageService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]

        //Buraya chatgroupId ile direkt mesaj gönderme işlemini düzelt
        public async Task<IActionResult> SendMessageToBinaryGroup(SendMessageDto sendMessageDto)
        {
            try
            {

                var chatGroupMembers = await chatGroupMemberService.GetAllAsync();
                var chatGroupMemberList = chatGroupMembers.Where(a => a.chatgroupId == sendMessageDto.chatGroupId).ToList();
                if (chatGroupMemberList.Count() == 2)
                {
                    var chatGroupMessage = new ChatGroupMessages
                    {
                        chatGroupMember =  chatGroupMemberList.Where(a=>a.memberUserId==sendMessageDto.currentUserId).FirstOrDefault(),
                        TochatGroupMemberId = chatGroupMemberList.Where(a => a.memberUserId == sendMessageDto.receiverClientId).FirstOrDefault().Id,
                        userId = chatGroupMemberList.Where(a => a.memberUserId == sendMessageDto.receiverClientId).FirstOrDefault().memberUserId,
                        ChatGroupId = sendMessageDto.chatGroupId.Value,
                        message = sendMessageDto.message,
                        isDelivered = true,
                        deliveredDate = DateTime.UtcNow,
                    };
                    bool result = await chatGroupMessageService.AddAsync(chatGroupMessage);
                    if (!result)
                    {

                        return BadRequest();


                    }
                    return Ok("Mesaj database kaydedildi");
                }
                else if (chatGroupMemberList.Count() > 2)
                {
                    return BadRequest("Bu bir binary grup olmalıydı bir hata olmalı!");
                }
                else if(chatGroupMemberList.Count() <2)
                {
                    return BadRequest("2 den az member olamaz.");

                }
                else return BadRequest("Beklenmedik bir hata oldu."); 
               
            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }

        }
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> SendMessageToGroupAsync(SendMessageDto sendMessageDto)
        //{
        //    try
        //    {
        //        var group = await chatgroupService.GetByIdAsync(sendMessageDto.groupId);
        //        await hubContext.Clients.Group(group.description).receiveMessage(sendMessageDto.message);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest($"An error occurred: {ex.Message}");
        //    }

      
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> getAllChatMessage(int ChatGroupId)
        {

            var listMessages = await chatGroupMessageService.GetAllAsync();
            var listMessageByGroupId = listMessages.Where(a => a.ChatGroupId== ChatGroupId).ToList();
            
            return Ok(listMessageByGroupId);
            
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> createGroup(CreateGroupDto createGroupDto)
        {
            
            try
            {
                ChatGroup chatGroup = new ChatGroup();
                mapper.Map(createGroupDto, chatGroup);
                var allChatGroups = await chatgroupService.GetAllAsync();
                if (allChatGroups.Select(a => a.description).Contains(chatGroup.description))
                {
                    return BadRequest("This Group has already created");
                }
                bool result= await chatgroupService.AddAsync(chatGroup);

                if (result)
                {
                    //await hubContext.Groups.AddToGroupAsync(createGroupDto.connectionId, createGroupDto.description);
                    var allGroups = await chatgroupService.GetAllAsync();
                    await hubContext.Clients.All.groups(allGroups);
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
                if (groupMembersDto.chatGroup != null && groupMembersDto.contactUser!=null)
                {
                    List<ChatGroupMember> members = new List<ChatGroupMember>();
                    var chatgroup = await chatgroupService.GetByIdAsync(groupMembersDto.chatGroup.Id);
                    var currentUser = userManager.Users.Where(a => a.Id == groupMembersDto.currentUserId).FirstOrDefault();
                    var currentUserBeContact=mapper.Map<User, ContactUsersDto>(currentUser);
                    List<ContactUsersDto> newList = new();
                    newList.Add(currentUserBeContact);
                    newList.Add(groupMembersDto.contactUser);
                    for (int i = 0; i < newList.Count(); i++)
                    {
                        User userDto = userManager.Users.Where(a => a.Id == newList[i].Id).FirstOrDefault();

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
        public async Task<IActionResult> GetAllGroupChats(int filteredId)
        {
            try
            {
                var chatGroups= await chatgroupService.GetAllAsync();
                var chatGroupsAll = chatGroups.Where(a => a.IsBinaryGroup == filteredId);
                return Ok(chatGroupsAll);

            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }


        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GroupsExistingControl(int ChatGroupId)
        {
            try
            {

                var list = await chatgroupService.GetAllAsync();
                var chatgroup = list.Where(a => a.Id == ChatGroupId).ToList();
                if (chatgroup.Count()>=0)
                {
                    return Ok(chatgroup);
                }
                return BadRequest(StatusCodes.Status404NotFound);

            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }


        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllContacts(string userid)
        {
            try
            {
                int count = 0;
                var userList = userManager.Users
                .Include(u => u.ChatGroupMembers)       
                .ToList();
                userList.Remove(userList.Where(x => x.Id == userid).ToList().FirstOrDefault());
                userList.Remove(userList.Where(a => a.Id == "7a84b14f-0397-4f30-bec5-cbeeab3e5fa0").FirstOrDefault());
                List<ContactUsersDto> contactUserList = new List<ContactUsersDto>();
                if (userList.Count >= 0)
                {
                    mapper.Map(userList, contactUserList);
                    foreach (User user in userList)
                    {
                        foreach (var members in user.ChatGroupMembers)
                        {
                            foreach (var item in contactUserList)
                            {
                                if (item.Id == user.Id)
                                {
                                    var listMember = await chatGroupMemberService.GetAllAsync(a => a.chatgroupId == members.chatgroupId);
                                    foreach (var chatgroupMember in listMember)
                                    {

                                        if (chatgroupMember.memberUserId == userid || chatgroupMember.memberUserId == user.Id)
                                        {
                                            count++;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    if (count == 2)
                                    {
                                        item.ChatGroupId = members.chatgroupId;
                                        
                                    }
                                    count = 0;
                                }
                                else
                                {
                                    continue;
                                }
                               

                            }
                        }
                    }
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

        [HttpPost]
        [Route("[action]")]

        //Buraya girmiyor sebebine bak!
        public async Task<IActionResult> GetUnreadedMessagesCount(UnreadedMessagesCountDto messagesCountDto)
        {
            try
            {
                var list = await chatGroupMessageService.GetAllAsync();
                var filteredListCount = list.Where(a => a.isRead == false && a.ChatGroupId == messagesCountDto.groupChatId && a.userId==messagesCountDto.currentUserId).Count();
                return Ok(filteredListCount);
                
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
