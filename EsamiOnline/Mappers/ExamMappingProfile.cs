using AutoMapper;
using EsamiOnline.Exam;
using EsamiOnline.Models;

namespace EsamiOnline.Mappers;

public class ExamMappingProfile : Profile
{
    public ExamMappingProfile()
    {
        // Map ExamRequest to ExamEntity
        CreateMap<ExamRequest, ExamEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ExamDateTime, opt => opt.MapFrom(src => src.ExamDatetime.ToDateTime()))
            .ForMember(dest => dest.MaxDuration, opt => opt.MapFrom(src => (decimal?)src.Duration.GetValueOrDefault()))
            .ForMember(dest => dest.BookedStudents, opt => opt.MapFrom(src => src.BookedStudents.GetValueOrDefault()));
    }
}