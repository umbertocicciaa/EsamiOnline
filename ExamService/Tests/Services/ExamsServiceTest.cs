using AutoMapper;
using EsamiOnline.Exam;
using ExamService.Models;
using ExamService.Repositories;
using ExamService.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using JetBrains.Annotations;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace ExamService.Tests.Services;

[TestSubject(typeof(ExamsService))]
public class ExamsServiceTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IExamRepository> _repositoryMock;
    private readonly ExamsService _examsService;

    public ExamsServiceTest()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<IExamRepository>();
        _examsService = new ExamsService(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task SaveExam_ShouldSaveExamAndReturnEmpty()
    {
        // Arrange
        var examRequest = new ExamDto
        {
            Name = "Math",
        };
        var examEntity = new ExamEntity
        {
            Name = "Math",
        };

        _mapperMock.Setup(m => m.Map<ExamEntity>(It.IsAny<ExamDto>())).Returns(examEntity);
        _repositoryMock.Setup(r => r.SaveExam(It.IsAny<ExamEntity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _examsService.SaveExam(examRequest, It.IsAny<ServerCallContext>());

        // Assert
        _mapperMock.Verify(m => m.Map<ExamEntity>(examRequest), Times.Once);
        _repositoryMock.Verify(r => r.SaveExam(examEntity), Times.Once);
        Assert.IsType<Empty>(result);
    }
    
    
    [Fact]
    public async Task GetExamByDate_ValidRequest_WritesExamsToStream()
    {
        // Arrange
        var responseStreamMock = new Mock<IServerStreamWriter<ExamDto>>();
        var examEntities = new List<ExamEntity> { new() { Name = "Ing", ExamDateTime = new BsonDateTime(new DateTime())}, new() {Name = "Math", ExamDateTime = new BsonDateTime(new DateTime())} };
        var examDtos = new List<ExamDto> { new() { Name = "Ing", ExamDatetime = new Timestamp()} , new() {Name = "Math", ExamDatetime = new Timestamp()} };
        var request = new ExamDateRequest { StartDate = new Timestamp(), EndDate = new Timestamp()};
        _repositoryMock.Setup(r => r.GetExamsByDate(It.IsAny<BsonDateTime>(), It.IsAny<BsonDateTime>())).Returns(examEntities);
        _mapperMock.Setup(m => m.Map<ExamDto>(It.IsAny<ExamEntity>())).Returns(examDtos.First());

        // Act
        await _examsService.GetExamByDate(request, responseStreamMock.Object, null!);

        // Assert
        responseStreamMock.Verify(s => s.WriteAsync(It.IsAny<ExamDto>()), Times.Exactly(examEntities.Count));
    }

    [Fact]
    public async Task GetExamByDate_NoExams_WritesNothingToStream()
    {
        // Arrange
        var responseStreamMock = new Mock<IServerStreamWriter<ExamDto>>();
        var request = new ExamDateRequest { StartDate = new Timestamp(), EndDate = new Timestamp()};
        _repositoryMock.Setup(r => r.GetExamsByDate(It.IsAny<BsonDateTime>(), It.IsAny<BsonDateTime>())).Returns(new List<ExamEntity>());
        var service = new ExamsService(_mapperMock.Object, _repositoryMock.Object);

        // Act
        await service.GetExamByDate(request, responseStreamMock.Object, null!);

        // Assert
        responseStreamMock.Verify(s => s.WriteAsync(It.IsAny<ExamDto>()), Times.Never);
    }

    [Fact]
    public async Task User_ShouldBookToExam()
    {
        // Arrange
        var student = new BookedStudent("ccc", "ddd");
        var exam = "Math";
        var bookRequest = new BookExamRequest
        {
            GovId = student.GovId,
            StudentId = student.StudentId,
            ExamName = exam
        };
        _repositoryMock.Setup(r => r.BookToExam(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new BoolValue { Value = true }));
        var service = new ExamsService(_mapperMock.Object, _repositoryMock.Object);
        
        // Act
        var response = await service.BookExam(bookRequest, It.IsAny<ServerCallContext>());
        
        Assert.True(response.Value);
    }
}