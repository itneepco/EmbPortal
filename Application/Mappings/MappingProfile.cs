using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Shared.Identity;
using Shared.Responses;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(m => m.EmployeeCode, opt => opt.MapFrom(p => p.UserName));

            CreateMap<Project, ProjectResponse>();

            CreateMap<Uom, UomResponse>()
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Dimension.ToString()));

            CreateMap<Contractor, ContractorResponse>();
        }
    }
}
