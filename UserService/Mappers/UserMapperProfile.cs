using AutoMapper;
using User;
using UserService.Models;

namespace UserService.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<UserDto, UserEntity>();
    }
}