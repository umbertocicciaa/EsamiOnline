using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Models;
using Google.Protobuf.WellKnownTypes;
using MongoDB.Bson;

namespace EsamiOnline.Mappers;

public class ExamMappingProfile : Profile
{
    public ExamMappingProfile()
    {
        // Map ExamRequest to ExamEntity
        CreateMap<ExamDto, ExamEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ExamDateTime, opt => opt.MapFrom(src => new BsonDateTime(src.ExamDatetime.ToDateTime())))
            .ForMember(dest => dest.MaxDuration, opt => opt.MapFrom(src => (decimal?)src.Duration.GetValueOrDefault()))
            .ForMember(dest => dest.BookedStudents, opt => opt.MapFrom(src => src.BookedStudents.GetValueOrDefault()));
        
        CreateMap<ExamEntity, ExamDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ExamDatetime, opt => opt.MapFrom(src => src.ExamDateTime != null ? Timestamp.FromDateTime(src.ExamDateTime.ToUniversalTime()) : null))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => (int)src.MaxDuration.GetValueOrDefault()))
            .ForMember(dest => dest.BookedStudents, opt => opt.MapFrom(src => src.BookedStudents.GetValueOrDefault()));
        
    }
}