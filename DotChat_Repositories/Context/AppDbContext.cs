using DotChat_Entities.DbSet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Repositories.Context
{
    public class AppDbContext: IdentityDbContext<User,Role,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext)
        {
            
        }

        public virtual DbSet<ChatGroup> chatGroups { get; set; }
        public virtual DbSet<ChatGroupMember> chatGroupMembers { get; set; }
        public virtual DbSet<ChatGroupMessages> chatGroupMessages { get; set; }
        public virtual DbSet<ChatGroupMemberInbox> ChatGroupMemberInboxes { get; set; }
        public virtual DbSet<ChatConnectionLog> ChatConnectionLogs { get; set; }
    }
    
}
