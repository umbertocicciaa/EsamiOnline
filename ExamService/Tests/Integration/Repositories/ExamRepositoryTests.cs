using ExamService.Models;
using ExamService.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace ExamService.Tests.Integration.Repositories;

public class ExamRepositoryTests : IDisposable
{
    private readonly ExamRepository _repository;
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();

    public ExamRepositoryTests()
    {
        _container.StartAsync().Wait();
        _repository =
            new ExamRepository(
                new MongoClient(_container.GetConnectionString()).GetDatabase($"test_db_{Guid.NewGuid()}"));
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

        await _repository.AddAsync(exam);
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

        await _repository.AddAsync(exam);

        var start = new BsonDateTime(DateTime.Now.AddHours(-1));
        var end = new BsonDateTime(DateTime.Now.AddHours(2));

        var exams = _repository.GetExamsByDate(start, end);

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

        await _repository.AddAsync(exam);
        var result = await _repository.BookToExam("Exam 1", "1", "1");

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

        await _repository.AddAsync(exam);
        await _repository.BookToExam("Exam 1", "1", "1");

        var updatedExam = await _repository.GetExamByName(exam.Name);
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

        await _repository.AddAsync(exam);
        await _repository.BookToExam("Exam 1", "1", "1");

        var updatedExam = await _repository.GetExamByName(exam.Name);
        Assert.Contains(new BookedStudent("1", "1"), updatedExam.InfoBookedStudents);
    }

    public async void Dispose()
    {
        await _container.DisposeAsync();
    }
}