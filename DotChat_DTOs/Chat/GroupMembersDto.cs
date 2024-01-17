using DotChat_Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class GroupMembersDto
    {
        public string? currentUserId { get; set; }

        public ChatGroup? chatGroup { get; set; }

        public ContactUsersDto? contactUser { get; set; }

    }
}
