using AutoMapper;
using EsamiOnline.Configs;
using EsamiOnline.Exam;
using EsamiOnline.Models;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EsamiOnline.Services;

public class ExamsService : Exams.ExamsBase
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<ExamEntity> _examsCollection;
    
    public ExamsService(IMapper mapper, IOptions<ExamOnlineDatabaseSettings> examsDatabaseSettings)
    {
        _mapper = mapper;
        var mongoClient = new MongoClient(
            examsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            examsDatabaseSettings.Value.DatabaseName);

        _examsCollection = mongoDatabase.GetCollection<ExamEntity>(
            examsDatabaseSettings.Value.ExamCollectionName);
    }
    
    public override Task<ExamReply> SaveExam(ExamRequest request, ServerCallContext context)
    {
        var exam = _mapper.Map<ExamEntity>(request);
        _examsCollection.InsertOneAsync(exam);
        return Task.FromResult(new ExamReply
        {
            Id = exam.Id.ToString(),
            Message = exam.Name
        });
    }
}