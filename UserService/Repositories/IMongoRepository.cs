using MongoDB.Bson;
using MongoDB.Driver;

namespace UserService.Repositories;

public interface IMongoRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(ObjectId id);
    Task AddAsync(T entity);
    Task UpdateAsync(ObjectId id, T entity);
    Task DeleteAsync(ObjectId id);
    Task<List<T>> GetAllWithFilterAsync(FilterDefinition<T> filter);
    Task<T> GetOneWithFilterAsync(FilterDefinition<T> filter);

}