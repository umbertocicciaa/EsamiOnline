using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Models;
using EsamiOnline.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EsamiOnline.Services;

public class ExamsService(IMapper mapper, IExamRepository repository) : Exams.ExamsBase
{
    public override async Task<Empty> SaveExam(ExamRequest request, ServerCallContext context)
    {
        var exam = mapper.Map<ExamEntity>(request);
        await repository.SaveExam(exam);
        return new Empty();
    }
}