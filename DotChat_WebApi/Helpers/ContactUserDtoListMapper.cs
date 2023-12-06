using AutoMapper;
using DotChat_DTOs.Chat;
using DotChat_Entities.DbSet;

namespace DotChat_WebApi.Helpers
{
    public class ContactUserDtoListMapper : ITypeConverter<List<ContactUsersDto>, List<User>>
    {
        public List<User> Convert(List<ContactUsersDto> source, List<User> destination, ResolutionContext context)
        {
            return context.Mapper.Map<List<User>>(source);
        }
    }
}
