using System;
using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Mappers;
using EsamiOnline.Models;
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
        var now = new DateTime();
        
        var request = new ExamRequest
        {
            Name =  "Physics Exam",
            ExamDatetime = now.Ticks,
            Duration = 120 ,
            BookedStudents = 30 ,
        };
        
        var entity = mapper.Map<ExamEntity>(request);

        Assert.Equal(request.Name, entity.Name);
        Assert.Equal(request.ExamDatetime, entity.ExamDateTime.GetValueOrDefault().Ticks);
        Assert.Equal((decimal)request.Duration.Value, entity.MaxDuration);
        Assert.Equal(request.BookedStudents.Value, entity.BookedStudents);
        Assert.Equal(default, entity.Id); 
    }
}