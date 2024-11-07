using ExamService.Models;
using Google.Protobuf.WellKnownTypes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExamService.Repositories;

public class ExamRepository(IMongoDatabase database) : MongoRepository<ExamEntity>(database, "Exams")
{
    
    public IEnumerable<ExamEntity> GetExamsByDate(BsonDateTime start, BsonDateTime end)
    {
        var filter = Builders<ExamEntity>.Filter.Gte(e => e.ExamDateTime, start) &
                     Builders<ExamEntity>.Filter.Lte(e => e.ExamDateTime, end);
        return Collection.Find(filter).ToCursor().ToList();
    }

    public async Task<BoolValue> BookToExam(string exam, string studentId, string govId)
    {
        var examEntity = Collection.Find(e => e.Name == exam).FirstOrDefault();
        if (examEntity == null) return new BoolValue { Value = false };
        var bookedNumber = examEntity.BookedStudents.GetValueOrDefault() + 1 ;
        var bookedExams = examEntity.InfoBookedStudents;
        bookedExams.Add(new BookedStudent(studentId, govId));
        var filter = Builders<ExamEntity>.Update
            .Set(e => e.BookedStudents, bookedNumber)
            .Set(e => e.InfoBookedStudents, bookedExams);
        await Collection.UpdateOneAsync(e => e.Name == exam, filter);
        return new BoolValue { Value = true };
    }

    public async Task<ExamEntity> GetExamByName(string name)
    {
        return await Collection.Find(e => e.Name == name).FirstOrDefaultAsync();
    }
}