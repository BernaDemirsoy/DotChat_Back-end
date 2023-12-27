using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatGroupMember:BaseEntity
    {
        public ChatGroupMember()
        {
            ChatGroupMessages =new HashSet<ChatGroupMessages>();
        }

        public int chatgroupId { get; set; }
        public virtual ChatGroup chatGroup { get; set; }

        public string memberUserId { get; set; }
        public virtual User user { get; set; }

        public bool isMemberChannelAdmin { get; set; }=false;
        public bool isChannelMutedByMember { get; set; } = false;
        public bool isMemberRemovedFromChannel { get; set; } = false;
        public DateTime? memberRemovedDate { get; set; }

        public virtual ICollection<ChatGroupMessages>? ChatGroupMessages { get; set; }

    }
}
