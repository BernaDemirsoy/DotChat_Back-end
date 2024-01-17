using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class CreateGroupDto
    {
        public string description { get; set; }
        public int IsBinaryGroup { get; set; }
        public string? groupAvatarImage { get; set; } = null;
    }
}
