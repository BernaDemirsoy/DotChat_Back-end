using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Account
{
    public class NewUserDto
    {
        public string userName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string passwordHash { get; set; } = null!;
        public string confirmedPasswordHash { get; set; } = null!;

    }
}
