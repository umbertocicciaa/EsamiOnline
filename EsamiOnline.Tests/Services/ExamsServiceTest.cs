using System;
using System.Collections.Generic;
using System.Linq;
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
        var examEntities = new List<ExamEntity> { new() { Name = "Ing", ExamDateTime = new DateTime()}, new() {Name = "Math", ExamDateTime = new DateTime()} };
        var examDtos = new List<ExamDto> { new() { Name = "Ing", ExamDatetime = new DateTime().Ticks} , new() {Name = "Math", ExamDatetime = new DateTime().Ticks} };
        var request = new ExamDateRequest { StartDate = new DateTime().Ticks, EndDate = new DateTime().Ticks };
        _repositoryMock.Setup(r => r.GetExamsByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(examEntities);
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
        var request = new ExamDateRequest { StartDate = new DateTime().Ticks, EndDate = new DateTime().Ticks};
        _repositoryMock.Setup(r => r.GetExamsByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<ExamEntity>());
        var service = new ExamsService(_mapperMock.Object, _repositoryMock.Object);

        // Act
        await service.GetExamByDate(request, responseStreamMock.Object, null!);

        // Assert
        responseStreamMock.Verify(s => s.WriteAsync(It.IsAny<ExamDto>()), Times.Never);
    }
}