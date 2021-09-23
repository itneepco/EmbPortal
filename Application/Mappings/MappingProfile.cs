using Application.Projects.Response;
using AutoMapper;
using Domian;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project,ProjectDto>();
        }
    }
}