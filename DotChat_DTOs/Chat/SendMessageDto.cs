using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class SendMessageDto
    {
        public string? receiverClientId { get; set; }
        public string? currentUserId { get; set; }
        public string? message { get; set; }
    }
}
