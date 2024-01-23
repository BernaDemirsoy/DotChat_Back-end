using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class ContactUsersDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        
        public string? avatarImage { get; set; }

        public bool? isAvatarImageSet { get; set; }

        public int? ChatGroupId { get; set; }
    }
}
