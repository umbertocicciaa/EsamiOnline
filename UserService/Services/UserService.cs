using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using User;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services;

public class UserService(IMapper mapper, IMongoRepository<UserEntity> repository) : User.User.UserBase
{
    public override Task<Empty> SaveUser(UserDto request, ServerCallContext context)
    {
        var user = mapper.Map<UserEntity>(request);
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        repository.AddAsync(user);
        return Task.FromResult(new Empty());
    }
    
    public override async Task GetUsers(Empty request, IServerStreamWriter<UserDto> responseStream, ServerCallContext context)
    {
        var users = await repository.GetAllAsync();
        foreach (var userDto in users.Select(mapper.Map<UserDto>))
            await responseStream.WriteAsync(userDto);
    }
}