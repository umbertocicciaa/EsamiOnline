using EsamiOnline.Configs;
using EsamiOnline.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EsamiOnline.Repositories;

public class ExamRepository : IExamRepository
{
    private readonly IMongoCollection<ExamEntity> _examsCollection;

    public ExamRepository(IOptions<ExamOnlineDatabaseSettings> examsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            examsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            examsDatabaseSettings.Value.DatabaseName);

        _examsCollection = mongoDatabase.GetCollection<ExamEntity>(
            examsDatabaseSettings.Value.ExamCollectionName);
    }

    public async Task SaveExam(ExamEntity exam)
    {
        await _examsCollection.InsertOneAsync(exam);
    }

    public IEnumerable<ExamEntity> GetExamsByDate(BsonDateTime start, BsonDateTime end)
    {    
        var filter = Builders<ExamEntity>.Filter.Gte(e => e.ExamDateTime, start) & Builders<ExamEntity>.Filter.Lte(e => e.ExamDateTime, end);
        return _examsCollection.Find(filter).ToCursor().ToList();
    }
}