
using AutoMapper;
using Domian;
using Shared.Response;

namespace Application.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project,ProjectResponse>();
        }
    }
}