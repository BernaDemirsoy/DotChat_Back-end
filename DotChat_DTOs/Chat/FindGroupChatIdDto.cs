using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class FindGroupChatIdDto
    {
        public string currentUserId { get; set; }
        public string clientUserId { get; set; }
    }
}
