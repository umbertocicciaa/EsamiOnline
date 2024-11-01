using ExamService.Models;
using ExamService.Repositories;
using ExamService.Tests.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Xunit;

namespace ExamService.Tests.Repositories;

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
        
        Assert.NotEmpty(exams);
    }
    
    [Fact]
    public async Task Should_Book_To_Exam()
    {
        var exam = new ExamEntity
        {
            Name = "Exam 1",
            ExamDateTime = new BsonDateTime(DateTime.Now),
            MaxDuration = 120,
            BookedStudents = 0
        };
        
        await _examRepository.SaveExam(exam);
        var result = await _examRepository.BookToExam("Exam 1", "1", "1");
        
        Assert.True(result.Value);
    }
    
    [Fact]
    public async Task Should_Increment_BookedStudent()
    {
        var exam = new ExamEntity
        {
            Name = "Exam 1",
            ExamDateTime = new BsonDateTime(DateTime.Now),
            MaxDuration = 120,
            BookedStudents = 0
        };
        
        await _examRepository.SaveExam(exam);
        await _examRepository.BookToExam("Exam 1", "1", "1");
        
        var updatedExam = await _examRepository.GetExamByName(exam.Name);
        Assert.Equal(1, updatedExam.BookedStudents);
    }
    
    [Fact]
    public async Task Should_Be_Inserted_ExamCollection()
    {
        var exam = new ExamEntity
        {
            Name = "Exam 1",
            ExamDateTime = new BsonDateTime(DateTime.Now),
            MaxDuration = 120,
            BookedStudents = 0
        };
        
        await _examRepository.SaveExam(exam);
        await _examRepository.BookToExam("Exam 1", "1", "1");
        
        var updatedExam = await _examRepository.GetExamByName(exam.Name);
        Assert.Contains(new BookedStudent("1", "1"), updatedExam.InfoBookedStudents);
    }
    
}