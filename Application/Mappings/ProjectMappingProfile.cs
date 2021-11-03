
using AutoMapper;
using Domain.Entities;
using Shared.Response;

namespace Application.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectResponse>();
        }
    }
}