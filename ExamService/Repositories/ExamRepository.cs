using ExamService.Configs;
using ExamService.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExamService.Repositories;

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

    public async Task<BoolValue> BookToExam(string exam, string studentId, string govId)
    {
        var examEntity = _examsCollection.Find(e => e.Name == exam).FirstOrDefault();
        if (examEntity == null) return new BoolValue { Value = false };
        examEntity.BookedStudents++;
        examEntity.InfoBookedStudents.Add(new BookedStudent(studentId, govId));
        await _examsCollection.UpdateOneAsync(e => e.Name == exam, Builders<ExamEntity>.Update.Set(e => e.BookedStudents, examEntity.BookedStudents).Set(e => e.InfoBookedStudents, examEntity.InfoBookedStudents));
        return new BoolValue{Value = true};
    }
    
    public async Task<ExamEntity> GetExamByName(string name)
    {
        return await _examsCollection.Find(e => e.Name == name).FirstOrDefaultAsync();
    }
}