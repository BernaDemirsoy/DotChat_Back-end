using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatGroupMessages:BaseEntity
    {
       
        public int chatGroupMemberId { get; set; }
        public virtual ChatGroupMember chatGroupMember { get; set; }

        public string message { get; set; }

        public DateTime messageTimestamp { get; set; }

        public bool isDeleted { get; set; } = false;

        public virtual ChatGroupMemberInbox chatGroupMemberInboxe { get; set; }
    }
}
