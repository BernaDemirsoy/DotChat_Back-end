using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class ChatConnectionLog:BaseEntity
    {
      
        public string userId { get; set; }
        public User user { get; set; }

        public string connectionId { get; set; }

        public DateTime connectedDate { get; set; }

        public DateTime disConnectedDate { get; set; }

    }
}
