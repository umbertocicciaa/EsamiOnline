using ExamService.Models;
using Google.Protobuf.WellKnownTypes;
using MongoDB.Bson;

namespace ExamService.Repositories;

public interface IExamRepository
{
    Task SaveExam (ExamEntity exam);
    IEnumerable<ExamEntity> GetExamsByDate(BsonDateTime start, BsonDateTime end);
    Task<BoolValue> BookToExam(string exam, string studentId, string govId);
    Task<ExamEntity> GetExamByName(string name);
}
