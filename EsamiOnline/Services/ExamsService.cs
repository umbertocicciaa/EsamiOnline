using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Models;
using EsamiOnline.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EsamiOnline.Services;

public class ExamsService(IMapper mapper, IExamRepository repository) : Exams.ExamsBase
{
    public override async Task<Empty> SaveExam(ExamDto request, ServerCallContext context)
    {
        var exam = mapper.Map<ExamEntity>(request);
        await repository.SaveExam(exam);
        return new Empty();
    }

    public override Task GetExamByDate(ExamDateRequest request, IServerStreamWriter<ExamDto> responseStream, ServerCallContext context)
    {
        var exams = repository.GetExamsByDate(new DateTime(request.StartDate.GetValueOrDefault()), new DateTime(request.EndDate.GetValueOrDefault()));
        foreach (var exam in exams)
        {
            var examDto = mapper.Map<ExamDto>(exam);
            responseStream.WriteAsync(examDto);
        }
        return Task.CompletedTask;
    }
}