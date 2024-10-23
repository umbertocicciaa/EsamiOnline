using System.Threading.Tasks;
using AutoMapper;
using EsamiOnline.Configs;
using EsamiOnline.Exam;
using EsamiOnline.Models;
using EsamiOnline.Services;
using Grpc.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace EsamiOnline.Tests.Services;

[TestSubject(typeof(ExamsService))]
public class ExamsServiceTest
{
    
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IOptions<ExamOnlineDatabaseSettings>> _mockOptions;
    private readonly Mock<IMongoCollection<ExamEntity>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly Mock<IMongoClient> _mockClient;
    private readonly ExamsService _service;
    
    public ExamsServiceTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockOptions = new Mock<IOptions<ExamOnlineDatabaseSettings>>();
        _mockCollection = new Mock<IMongoCollection<ExamEntity>>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();

        _mockOptions.Setup(opt => opt.Value).Returns(new ExamOnlineDatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "ExamsDatabase",
            ExamCollectionName = "Exams"
        });

        _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(_mockDatabase.Object);
        _mockDatabase.Setup(db => db.GetCollection<ExamEntity>(It.IsAny<string>(), null)).Returns(_mockCollection.Object);

        _service = new ExamsService(_mockMapper.Object, _mockOptions.Object);
    }

    [Fact]
    public async Task SaveExamTest()
    {
        // Arrange
        var request = new ExamRequest { Name = "Math Exam" };
        var context = new Mock<ServerCallContext>();
        var examEntity = new ExamEntity { Id = new MongoDB.Bson.ObjectId(), Name = "Math Exam" };

        _mockMapper.Setup(m => m.Map<ExamEntity>(It.IsAny<ExamRequest>())).Returns(examEntity);
        _mockCollection.Setup(c => c.InsertOneAsync(It.IsAny<ExamEntity>(), null, default)).Returns(Task.CompletedTask);

        // Act
        var response = await _service.SaveExam(request, context.Object);

        // Assert
        Assert.Equal(examEntity.Id.ToString(), response.Id);
        Assert.Equal(examEntity.Name, response.Message);
    }
}