using AutoMapper;
using DotChat_DTOs.Account;
using DotChat_Entities.DbSet;

namespace DotChat_WebApi.Profiles
{
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            CreateMap<LoginDto, User>().ReverseMap();
            CreateMap<NewUserDto, User>().ReverseMap();
            CreateMap<SetAvatarDto, User>().ReverseMap();
        }
    }
}
