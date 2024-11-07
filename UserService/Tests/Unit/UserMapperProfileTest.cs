using AutoMapper;
using JetBrains.Annotations;
using MongoDB.Bson;
using User;
using UserService.Mappers;
using UserService.Models;
using Xunit;

namespace UserService.Tests.Unit;

[TestSubject(typeof(UserMapperProfile))]
public class UserMapperProfileTest
{
    private readonly IMapper _mapper;

    public UserMapperProfileTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapperProfile>());
        _mapper = config.CreateMapper();
    }
    
    [Fact]
    public void Map_UserDto_To_UserEntity()
    {
        var userDto = new UserDto
        {
            Name = "John",
            Surname = "asas",
            Email = "dasd",
            GovId = "2e",
            Password = "as",
            Role = "xc",
            Phone = "z"
        };
        
        var userEntity = _mapper.Map<UserEntity>(userDto);
        Assert.Equal(userEntity.Name, userDto.Name);
        Assert.Equal(userEntity.Surname, userDto.Surname);
        Assert.Equal(userEntity.Email, userDto.Email);
        Assert.Equal(userEntity.GovId, userDto.GovId);
        Assert.Equal(userEntity.Password, userDto.Password);
        Assert.Equal(userEntity.Role, userDto.Role);
        Assert.Equal(userEntity.Phone, userDto.Phone);
    }
    
    [Fact]
    public void Map_UserEntity_To_UserDto()
    {
        var userEntity = new UserEntity
        {
            Id = ObjectId.GenerateNewId(),
            Name = "John",
            Surname = "Comp",
            Email = "umb",
            GovId = "pippo",
            Password = "dsda",
            Role = "lol",
            Phone = "2psd"
        };
        
        var userDto = _mapper.Map<UserDto>(userEntity);
        Assert.Equal(userEntity.Name, userDto.Name);
        Assert.Equal(userEntity.Surname, userDto.Surname);
        Assert.Equal(userEntity.Email, userDto.Email);
        Assert.Equal(userEntity.GovId, userDto.GovId);
        Assert.Equal(userEntity.Password, userDto.Password);
        Assert.Equal(userEntity.Role, userDto.Role);
        Assert.Equal(userEntity.Phone, userDto.Phone);
    }
}