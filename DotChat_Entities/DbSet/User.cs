using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class User: IdentityUser
    {
        public User()
        {
            chatConnectionLogs=new HashSet<ChatConnectionLog>();
        }
        public string confirmedPasswordHash { get; set; }
        public string? avatarImage { get; set; }
        public bool? isAvatarImageSet { get; set; }

        public string? connectionId { get; set; }
        public virtual ICollection<ChatConnectionLog>? chatConnectionLogs { get; set; }
    }
}
