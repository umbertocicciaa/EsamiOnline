using EsamiOnline.Models;

namespace EsamiOnline.Repositories;

public interface IExamRepository
{
    Task SaveExam (ExamEntity exam);
}