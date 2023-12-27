using DotChat_Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Repositories.Abstract
{
    public interface IMessageClient
    {
        Task UserJoin(string connectionId);
       
        Task receiveMessage(string message);

        Task groups(List<ChatGroup> groups);

        Task receiveConnectionId(string connectionId);

        Task UserLeaved(string connectionId);
    }
}
