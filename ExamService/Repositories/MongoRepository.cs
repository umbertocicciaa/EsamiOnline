using MongoDB.Bson;
using MongoDB.Driver;

namespace ExamService.Repositories;

public class MongoRepository<T>(IMongoDatabase database, string collectionName) : IMongoRepository<T>
    where T : class
{
    protected readonly IMongoCollection<T> Collection = database.GetCollection<T>(collectionName);

    public async Task<List<T>> GetAllAsync()
    {
        return await Collection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<T> GetByIdAsync(ObjectId id)
    {
        return await Collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(ObjectId id, T entity)
    {
        await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
    }

    public async Task DeleteAsync(ObjectId id)
    {
        await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
    }
}