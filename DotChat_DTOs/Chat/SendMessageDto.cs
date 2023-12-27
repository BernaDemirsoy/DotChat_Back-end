using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class SendMessageDto
    {
        public int groupId { get; set; }

        public string message { get; set; }
    }
}
