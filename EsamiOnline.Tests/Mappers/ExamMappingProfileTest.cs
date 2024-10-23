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

        var request = new ExamRequest { Name = "Physics Exam" };
        var entity = mapper.Map<ExamEntity>(request);

        Assert.Equal(request.Name, entity.Name);
        Assert.Equal(default, entity.Id); // Id should be default as it is ignored in mapping
    }

    [Fact]
    public void ExamMappingProfile_MapsExamEntityToExamReply()
    {
        var mapper = _config.CreateMapper();

        var entity = new ExamEntity { Id = new MongoDB.Bson.ObjectId(), Name = "Chemistry Exam" };
        var reply = mapper.Map<ExamReply>(entity);

        Assert.Equal(entity.Id.ToString(), reply.Id);
        Assert.Equal(entity.Name, reply.Message);
    }

    [Fact]
    public void ExamMappingProfile_MapsExamEntityToExamRequest()
    {
        var mapper = _config.CreateMapper();

        var entity = new ExamEntity { Name = "Biology Exam" };
        var request = mapper.Map<ExamRequest>(entity);

        Assert.Equal(entity.Name, request.Name);
    }

    [Fact]
    public void ExamMappingProfile_MapsExamReplyToExamEntity()
    {
        var mapper = _config.CreateMapper();

        var reply = new ExamReply { Id = "507f1f77bcf86cd799439011", Message = "History Exam" };
        var entity = mapper.Map<ExamEntity>(reply);

        Assert.Equal(reply.Id, entity.Id.ToString());
        Assert.Equal(reply.Message, entity.Name);
    }
}