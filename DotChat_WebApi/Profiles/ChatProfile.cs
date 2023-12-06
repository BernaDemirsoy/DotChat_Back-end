using AutoMapper;
using DotChat_DTOs.Chat;
using DotChat_Entities.DbSet;
using DotChat_WebApi.Helpers;

namespace DotChat_WebApi.Profiles
{
    public class ChatProfile:Profile
    {
        public ChatProfile()
        {
            CreateMap<ContactUsersDto, User>().ReverseMap();
            CreateMap<List<ContactUsersDto>, List<User>>().ConvertUsing<ContactUserDtoListMapper>();
        }
    }
}
