using EsamiOnline.Models;
using MongoDB.Bson;

namespace EsamiOnline.Repositories;

public interface IExamRepository
{
    Task SaveExam (ExamEntity exam);
    IEnumerable<ExamEntity> GetExamsByDate(BsonDateTime start, BsonDateTime end);
}