using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatGroupMemberInbox:BaseEntity
    {
        public int TochatGroupMemberId { get; set; }
       
        public int chatGroupMessagesId { get; set; }
        public virtual ChatGroupMessages chatGroupMessages { get; set; }

        public int chatGroupId { get; set; }
        public string userId { get; set; }
        public bool isRead { get; set; } = false;

        public bool isDelivered { get; set; } = false;

        public DateTime? readDate { get; set; }

        public bool isArchived { get; set; } = false;
        public DateTime? archiveDate { get; set; }
        
    }
}
