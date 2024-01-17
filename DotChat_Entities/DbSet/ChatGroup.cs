using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatGroup:BaseEntity
    {
        public ChatGroup()
        {
            chatGroupMembers=new HashSet<ChatGroupMember>();
            
        }
        public string description { get; set; }
        public string? groupAvatarImage { get; set; }
        public bool isChannelClosed { get; set; } = false;
        public DateTime? channelCloseDate { get; set; }

        public string? channelCloseUserId { get; set; }
        public virtual User? user { get; set; }
        public int IsBinaryGroup { get; set; }

        [JsonIgnore]
        public virtual ICollection<ChatGroupMember>? chatGroupMembers { get; set; }
        
    }
}
