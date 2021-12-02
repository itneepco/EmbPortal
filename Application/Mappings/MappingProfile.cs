using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.WorkOrderAggregate;
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
                .ForMember(m => m.DimensionId, opt => opt.MapFrom(p => p.Dimension))
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Dimension.ToString()));

            CreateMap<Contractor, ContractorResponse>();

            CreateMap<WorkOrderItem, WorkOrderItemResponse>();

            CreateMap<WorkOrder, WorkOrderResponse>()
                .ForMember(m => m.ProjectName, opt => opt.MapFrom(p => p.Project.Name))
                .ForMember(m => m.ContractorName, opt => opt.MapFrom(p => p.Contractor.Name));
        }
    }
}
