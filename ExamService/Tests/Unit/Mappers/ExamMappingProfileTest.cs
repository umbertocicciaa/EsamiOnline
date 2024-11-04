using AutoMapper;
using EsamiOnline.Exam;
using ExamService.Mappers;
using ExamService.Models;
using Google.Protobuf.WellKnownTypes;
using JetBrains.Annotations;
using MongoDB.Bson;
using Xunit;

namespace ExamService.Tests.Unit.Mappers;

[TestSubject(typeof(ExamMappingProfile))]
public class ExamMappingProfileTest
{
    private readonly IMapper _mapper;

    public ExamMappingProfileTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ExamMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_ExamDtoToExamEntity_MapsCorrectly()
    {
        var examDto = new ExamDto
        {
            Name = "Math Exam",
            ExamDatetime = Timestamp.FromDateTime(DateTime.UtcNow),
            Duration = 120,
            BookedStudents = 30
        };

        var examEntity = _mapper.Map<ExamEntity>(examDto);
        Assert.Equal(examDto.Name, examEntity.Name);
        Assert.Equal(new BsonDateTime(examDto.ExamDatetime.ToDateTime()), examEntity.ExamDateTime);
        Assert.Equal((decimal?)examDto.Duration, examEntity.MaxDuration);
        Assert.Equal(examDto.BookedStudents, examEntity.BookedStudents);
    }

    [Fact]
    public void Map_ExamEntityToExamDto_MapsCorrectly()
    {
        var examEntity = new ExamEntity
        {
            Id = ObjectId.GenerateNewId(),
            Name = "Math Exam",
            ExamDateTime = new BsonDateTime(DateTime.UtcNow),
            MaxDuration = 120,
            BookedStudents = 30
        };

        var examDto = _mapper.Map<ExamDto>(examEntity);

        Assert.Equal(examEntity.Name, examDto.Name);
        Assert.Equal(Timestamp.FromDateTime(examEntity.ExamDateTime.ToUniversalTime()), examDto.ExamDatetime);
        Assert.Equal((int)examEntity.MaxDuration, examDto.Duration);
        Assert.Equal(examEntity.BookedStudents, examDto.BookedStudents);
    }
}