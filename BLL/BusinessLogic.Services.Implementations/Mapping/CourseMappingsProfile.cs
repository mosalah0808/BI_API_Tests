using AutoMapper;
using BusinessLogic.Contracts;
using DataAccess.Entities;

namespace BusinessLogic.Services.Mapping
{
    /// <summary>
    /// Профиль автомаппера для сущности курса.
    /// </summary>
    public class CourseMappingsProfile : Profile
    {
        public CourseMappingsProfile()
        {
            CreateMap<Course, CourseDto>();
            
            CreateMap<CourseDto, Course>()
                .ForMember(d => d.Lessons, map => map.Ignore());
        }
    }
}
