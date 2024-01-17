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
        private readonly IGenericService<ChatGroupMemberInbox> chatGroupMemberInboxService;
        private readonly IMapper mapper;
  

        public ChatController(UserManager<User> userManager, IHubContext<MessageHub, IMessageClient> hubContext, IGenericService<ChatConnectionLog> chatConnectionService, IGenericService<ChatGroup> chatgroupService, IGenericService<ChatGroupMember> chatGroupMemberService, IGenericService<ChatGroupMessages> chatGroupMessageService, IGenericService<ChatGroupMemberInbox> chatGroupMemberInboxService,IMapper mapper)
        {

            this.userManager = userManager;
            this.hubContext = hubContext;
            this.chatConnectionService = chatConnectionService;
            this.chatgroupService = chatgroupService;
            this.chatGroupMemberService = chatGroupMemberService;
            this.chatGroupMessageService = chatGroupMessageService;
            this.chatGroupMemberInboxService = chatGroupMemberInboxService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SendMessageToBinaryGroup(SendMessageDto sendMessageDto)
        {
            try
            {
                var chatGroupMembers = await chatGroupMemberService.GetAllAsync();
                var chatGroupIds = chatGroupMembers.Where(a => a.memberUserId == sendMessageDto.currentUserId).ToList();
                foreach (var chatGroupId in chatGroupIds)
                {
                    foreach (var member in chatGroupMembers)
                    {
                       if(member.chatgroupId== chatGroupId.chatgroupId && member.memberUserId == sendMessageDto.receiverClientId)
                        {
                            var chatGroupMessage = new ChatGroupMessages
                            {
                                chatGroupMember = member,
                                chatGroupMemberId = member.Id,
                                message = sendMessageDto.message,
                                messageTimestamp = DateTime.Now,
                            };

                            bool result=await chatGroupMessageService.AddAsync(chatGroupMessage);
                            if (!result)
                            {

                                return BadRequest();
                                
                                
                            }
                            var chatgroupInbox = new ChatGroupMemberInbox
                            {
                                chatGroupMemberId = member.Id,
                                chatGroupMessagesId = chatGroupMessage.Id,
                                chatGroupMessages = chatGroupMessage,

                            };
                            bool resultSecond = await chatGroupMemberInboxService.AddAsync(chatgroupInbox);
                            if (!resultSecond)
                            {
                                return BadRequest();
                            }
                        };
                    }
                }
                return Ok("Mesaj database kaydedildi.");
                
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

        //}
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> findGroupChatIdByClientId(FindGroupChatIdDto findGroupChatIdDto)
        {
            var chatGroupMembers = await chatGroupMemberService.GetAllAsync();
            var chatGroupIds = chatGroupMembers.Where(a => a.memberUserId == findGroupChatIdDto.currentUserId).ToList();
            int findedChatGroupId=0;
            foreach (var chatGroupId in chatGroupIds)
            {
                foreach (var member in chatGroupMembers)
                {
                    if (member.chatgroupId == chatGroupId.chatgroupId && member.memberUserId == findGroupChatIdDto.clientUserId)
                    {
                        findedChatGroupId=chatGroupId.chatgroupId;
                        break;
                    }
                }
            }
            var userList=chatGroupMembers.Where(a => a.chatgroupId == findedChatGroupId).ToList();
            int currentUserMemberId=0;
            int clientUserMemberId=0;
            foreach (var user in userList)
            {
                if(user.memberUserId== findGroupChatIdDto.clientUserId)
                {
                    clientUserMemberId=user.Id;
                }
                else if(user.memberUserId == findGroupChatIdDto.currentUserId)
                {
                    currentUserMemberId=user.Id;
                }
            }
            if(currentUserMemberId!=null && clientUserMemberId != null)
            {
                var viewData = new
                {
                    clientUserMemberId=clientUserMemberId,
                    currentUserMemberId=currentUserMemberId,
                };
                return Ok(viewData);
            }
            else
            {
                return BadRequest("Userların memberIdleri bulunamadı");
            }
           
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> getAllChatMessage(FindGroupChatIdDto findGroupChatIdDto)
        {
            var allMessages = await chatGroupMemberInboxService.GetAllAsync(a=>a.chatGroupMessages);
            var chatGroupMembers = await chatGroupMemberService.GetAllAsync();
            var chatGroupIds = chatGroupMembers.Where(a => a.memberUserId == findGroupChatIdDto.currentUserId).ToList();
            int findedChatGroupId = 0;
            foreach (var chatGroupId in chatGroupIds)
            {
                foreach (var member in chatGroupMembers)
                {
                    if (member.chatgroupId == chatGroupId.chatgroupId && member.memberUserId == findGroupChatIdDto.clientUserId)
                    {
                        findedChatGroupId = chatGroupId.chatgroupId;
                        break;
                    }
                }
            }
            var chatmembers=await chatGroupMemberService.GetAllAsync(a=>a.chatGroup);
            var listChatMember = chatmembers.Where(a => a.chatgroupId == findedChatGroupId);
            var listMessages = new List<ChatGroupMemberInbox>();
            foreach (var member in listChatMember)
            {
                var searchedUser = allMessages.Where(a => a.chatGroupMemberId == member.Id).FirstOrDefault();
                if (searchedUser!=null)
                {
                    listMessages.Add(searchedUser);
                }
            }
            return Ok(listMessages);
            
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
        public async Task<IActionResult> GetAllGroupChats()
        {
            try
            {
                var chatGroups= await chatgroupService.GetAllAsync();
                var chatGroupsAll = chatGroups.Where(a => a.IsBinaryGroup == 0);
                return Ok(chatGroupsAll);

            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred: {ex.Message}");
            }


        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GroupsExistingControl(string description)
        {
            try
            {
                
                string[] names=description.Split(new string[] { "//" }, StringSplitOptions.None);
                var chatGroups = await chatGroupMemberService.GetAllAsync(a => a.chatGroup);
                var list=chatGroups.Select(a=>a.chatGroup).Where(a => a.description.Contains(names[0]) && a.description.Contains(names[1])).ToList();
                if (list.Count() >= 0)
                {
                    return Ok(list);
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
                var userList = userManager.Users.ToList();
                userList.Remove(userList.Where(x => x.Id == userid).ToList().FirstOrDefault());
                userList.Remove(userList.Where(a => a.Id == "7a84b14f-0397-4f30-bec5-cbeeab3e5fa0").FirstOrDefault());
                List<ContactUsersDto> contactUserList = new List<ContactUsersDto>();
                if (userList.Count >= 0)
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
