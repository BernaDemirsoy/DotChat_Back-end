using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatGroupMessages:BaseEntity
    {
       
        public int TochatGroupMemberId { get; set; }
        public virtual ChatGroupMember chatGroupMember { get; set; }

        public int ChatGroupId { get; set; }

        public string message { get; set; }

        public string userId { get; set; }

        public bool isDeleted { get; set; } = false;

        public bool isRead { get; set; } = false;

        public bool isDelivered { get; set; } = false;

        public bool isArchived { get; set; } = false;
        public DateTime? deliveredDate { get; set; }

        public DateTime? readDate { get; set; }

        public DateTime? deletedDate { get; set; }
        public DateTime? archiveDate { get; set; }

    }
}
