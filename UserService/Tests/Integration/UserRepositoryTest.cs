using JetBrains.Annotations;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using UserService.Models;
using UserService.Repositories;
using Xunit;

namespace UserService.Tests.Integration;

[TestSubject(typeof(UserRepository))]
public class UserRepositoryTest : IDisposable
{

    private readonly UserRepository _repository;
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();
    
    public UserRepositoryTest()
    {
        _container.StartAsync().Wait();
        _repository =
            new UserRepository(
                new MongoClient(_container.GetConnectionString()).GetDatabase($"test_db_{Guid.NewGuid()}"));
    }
    
    public async void Dispose()
    {
        await _container.DisposeAsync();
    }
    
    [Fact]
    public async void Should_Add_User()
    {
        var user = new UserEntity
        {
            Name = "John Doe",
        };
        
        await _repository.AddAsync(user);
    }
    
    [Fact]
    public async void Should_Get_All_Users()
    {
        var user = new UserEntity
        {
            Name = "John Doe",
        };
        var user2 = new UserEntity
        {
            Name = "psdsa Doe",
        };
        await _repository.AddAsync(user);
        await _repository.AddAsync(user2);
        
        var users = await _repository.GetAllAsync();
        
        Assert.Equal(2, users.Count);
    }
}