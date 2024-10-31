using EsamiOnline.IntegrationTests.Configs;
using EsamiOnline.Models;
using EsamiOnline.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace EsamiOnline.IntegrationTests.Repositories;

public class ExamRepositoryTests : IClassFixture<MongoDbFixture>
{
    private readonly IExamRepository _examRepository;
    
    public ExamRepositoryTests(MongoDbFixture fixture)
    {
        var options = Options.Create(fixture.DbContextSettings);
        _examRepository = new ExamRepository(options);
    }
    
    [Fact]
    public async Task Should_Save_Entity()
    {
       var exam = new ExamEntity
       {
           Name = "Exam 1",
           ExamDateTime = new BsonDateTime(DateTime.Now),
           MaxDuration = 120,
           BookedStudents = 0
       };
       
       await _examRepository.SaveExam(exam);
    }
    
    [Fact]
    public async Task Should_Get_Exams_By_Date()
    {
        var exam = new ExamEntity
        {
            Name = "Exam 1",
            ExamDateTime = new BsonDateTime(DateTime.Now),
            MaxDuration = 120,
            BookedStudents = 0
        };
        
        await _examRepository.SaveExam(exam);
        
        var start = new BsonDateTime(DateTime.Now.AddHours(-1));
        var end = new BsonDateTime(DateTime.Now.AddHours(2));
        
        var exams = _examRepository.GetExamsByDate(start, end);
        
        Assert.Single(exams);
    }
    
}