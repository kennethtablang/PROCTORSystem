using AutoMapper;
using PROCTORSystem.DTO;
using PROCTORSystem.Models;

namespace PROCTORSystem.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
