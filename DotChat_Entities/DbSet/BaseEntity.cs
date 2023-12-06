using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Entities.DbSet
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
