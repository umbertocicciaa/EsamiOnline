using System.Threading.Tasks;
using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Models;
using EsamiOnline.Repositories;
using EsamiOnline.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using JetBrains.Annotations;
using Moq;
using Xunit;


namespace EsamiOnline.Tests.Services;

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
        var examRequest = new ExamRequest
        {
            Name = "Math",
        };
        var examEntity = new ExamEntity
        {
            Name = "Math",
        };

        _mapperMock.Setup(m => m.Map<ExamEntity>(It.IsAny<ExamRequest>())).Returns(examEntity);
        _repositoryMock.Setup(r => r.SaveExam(It.IsAny<ExamEntity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _examsService.SaveExam(examRequest, It.IsAny<ServerCallContext>());

        // Assert
        _mapperMock.Verify(m => m.Map<ExamEntity>(examRequest), Times.Once);
        _repositoryMock.Verify(r => r.SaveExam(examEntity), Times.Once);
        Assert.IsType<Empty>(result);
    }
}