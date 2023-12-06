using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_DTOs.Account
{
    public class SetAvatarDto
    {

        public string avatarImage { get; set; }

        public bool isAvatarImageSet { get; set; } = false;
    }
}
