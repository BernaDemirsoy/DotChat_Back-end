using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class CreateGroupDto
    {
        public string connectionId { get; set; }
        public string description { get; set; }
        public string? groupAvatarImage { get; set; } = null;
    }
}
