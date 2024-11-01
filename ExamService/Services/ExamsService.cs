using AutoMapper;
using EsamiOnline.Exam;
using ExamService.Models;
using ExamService.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MongoDB.Bson;

namespace ExamService.Services;

public class ExamsService(IMapper mapper, IExamRepository repository) : Exams.ExamsBase
{
    public override async Task<Empty> SaveExam(ExamDto request, ServerCallContext context)
    {
        var exam = mapper.Map<ExamEntity>(request);
        await repository.SaveExam(exam);
        return new Empty();
    }

    public override async Task GetExamByDate(ExamDateRequest request, IServerStreamWriter<ExamDto> responseStream, ServerCallContext context)
    {
        var exams = repository.GetExamsByDate
        (
            new BsonDateTime(request.StartDate.ToDateTime()), 
            new BsonDateTime(request.EndDate.ToDateTime())
        );
            
        foreach (var exam in exams)
        {
            var examDto = mapper.Map<ExamDto>(exam);
            await responseStream.WriteAsync(examDto);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}