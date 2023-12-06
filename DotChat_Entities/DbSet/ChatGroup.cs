using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string? connectionId { get; set; }
        public bool isChannelClosed { get; set; } = false;
        public DateTime? channelCloseDate 
        {
            get { return channelCloseDate; }
            set
            {
                if(isChannelClosed)
                    channelCloseDate = DateTime.UtcNow;
                else channelCloseDate = null;
            }
        }

        public string? channelCloseUserId { get; set; }
        public virtual User? user { get; set; }

        public virtual ICollection<ChatGroupMember> chatGroupMembers { get; set; }
    }
}
