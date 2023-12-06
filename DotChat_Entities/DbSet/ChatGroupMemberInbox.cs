using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatGroupMemberInbox:BaseEntity
    {
       
        public int chatGroupMemberId 
        {
            get
            {
                return chatGroupMemberId;
            }
            set
            {
                chatGroupMemberId = chatGroupMessages.chatGroupMemberId;
            } 
        }

        public int chatGroupMessagesId
        { get { return chatGroupMessagesId; }
            set
            {
                chatGroupMessagesId = chatGroupMessages.Id;
            } 
        }
        public virtual ChatGroupMessages chatGroupMessages { get; set; }

        public bool isRead { get; set; } = false;
        public DateTime? readDate{
            get { return readDate; }
            set
            {
                if (isRead)
                    readDate = DateTime.UtcNow;
                else
                    readDate = null;
            } 
        }

        public bool isArchived { get; set; } = false;
        public DateTime? archiveDate
        {
            get { return readDate; }
            set
            {
                if (isArchived)
                    archiveDate = DateTime.UtcNow;
                else
                    archiveDate = null;
            }
        }
    }
}
