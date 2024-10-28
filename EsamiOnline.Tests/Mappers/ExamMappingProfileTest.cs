using System;
using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Mappers;
using EsamiOnline.Models;
using Google.Protobuf.WellKnownTypes;
using JetBrains.Annotations;
using Xunit;

namespace EsamiOnline.Tests.Mappers;

[TestSubject(typeof(ExamMappingProfile))]
public class ExamMappingProfileTest
{
    
    private readonly MapperConfiguration _config;

    public ExamMappingProfileTest()
    {
        _config = new MapperConfiguration(cfg => cfg.AddProfile<ExamMappingProfile>());
    }

    [Fact]
    public void ExamMappingProfile_MapsExamRequestToExamEntity()
    {
        var mapper = _config.CreateMapper();
        var now = DateTime.UtcNow;
        
        var request = new ExamRequest
        {
            Name =  "Physics Exam",
            ExamDatetime = now.ToTimestamp(),
            Duration = 120 ,
            BookedStudents = 30 ,
        };
        
        var entity = mapper.Map<ExamEntity>(request);

        Assert.Equal(request.Name, entity.Name);
        Assert.Equal(request.ExamDatetime.ToDateTime(), entity.ExamDateTime);
        Assert.Equal((decimal)request.Duration.Value, entity.MaxDuration);
        Assert.Equal(request.BookedStudents.Value, entity.BookedStudents);
        Assert.Equal(default, entity.Id); 
    }
}