using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class UnreadedMessagesCountDto
    {
        public int groupChatId { get; set; }
        public string? currentUserId { get; set; }
        public string? contactName { get; set; }
        public string? receiverUserId { get; set; }

    }
}
