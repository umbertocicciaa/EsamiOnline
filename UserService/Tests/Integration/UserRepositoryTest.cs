using JetBrains.Annotations;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using UserService.Models;
using UserService.Repositories;
using Xunit;

namespace UserService.Tests.Integration;

[TestSubject(typeof(MongoRepository<>))]
public class UserRepositoryTest : IDisposable
{

    private readonly IMongoRepository<UserEntity> _repository;
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();
    
    public UserRepositoryTest()
    {
        _container.StartAsync().Wait();
        _repository = new MongoRepository<UserEntity>(new MongoClient(_container.GetConnectionString()).GetDatabase($"test_db_{Guid.NewGuid()}"),"Users");
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
    
    [Fact]
    public async void Should_Get_User_By_Gov_Id()
    {
        var user = new UserEntity
        {
            Name = "John Doe",
            GovId = "1234567890"
        };
        var user2 = new UserEntity
        {
            Name = "psdsa Doe",
            GovId = "1234567890"
        };
        await _repository.AddAsync(user);
        await _repository.AddAsync(user2);
        
        var filter = Builders<UserEntity>.Filter.Eq("GovId", "1234567890");
        var users = await _repository.GetAllWithFilterAsync(filter);
        
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public async void Should_Get_User_By_Student_Id()
    {
        var user = new StudentEntity()
        {
            Name = "John Doe",
            StudentId = "1234567890"
        };
        var user2 = new StudentEntity
        {
            Name = "psdsa Doe",
            StudentId = "1234567890"
        };
        await _repository.AddAsync(user);
        await _repository.AddAsync(user2);
        
        var filter = Builders<UserEntity>.Filter.Eq("StudentId", "1234567890");
        var users = await _repository.GetAllWithFilterAsync(filter);
        
        Assert.Equal(2, users.Count);
    }
}