using MongoDB.Driver;
using UserService.Models;

namespace UserService.Repositories;

public class UserRepository(IMongoDatabase database) : MongoRepository<UserEntity>(database, "Users")
{
    public new async Task AddAsync(UserEntity entity)
    {
        await Collection.InsertOneAsync(entity);
    } 
}