using ExamService.Models;
using MongoDB.Bson;

namespace ExamService.Repositories;

public interface IExamRepository
{
    Task SaveExam (ExamEntity exam);
    IEnumerable<ExamEntity> GetExamsByDate(BsonDateTime start, BsonDateTime end);
}