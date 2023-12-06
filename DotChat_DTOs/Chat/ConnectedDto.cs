using DotChat_Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Chat
{
    public class ConnectedDto
    {
        public User user { get; set; }

        public string connectionId { get; set; }
    }
}
