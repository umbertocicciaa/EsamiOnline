using EsamiOnline.Configs;
using EsamiOnline.Models;
using Microsoft.Extensions.Options;
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
}