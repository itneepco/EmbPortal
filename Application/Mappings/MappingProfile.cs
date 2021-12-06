using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Identity;
using EmbPortal.Shared.Responses;

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

            CreateMap<WorkOrder, WorkOrderResponse>()
                .ForMember(m => m.ProjectName, opt => opt.MapFrom(p => p.Project.Name))
                .ForMember(m => m.ContractorName, opt => opt.MapFrom(p => p.Contractor.Name))
                .ForMember(m => m.Status, opt => opt.MapFrom(p => p.Status.ToString()));

            CreateMap<WorkOrderItem, WorkOrderItemResponse>();

            CreateMap<SubItem, SubItemResponse>()
                .ForMember(m => m.Uom, opt => opt.MapFrom(p => p.Uom.Name))
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Uom.Dimension.ToString()));

            CreateMap<MeasurementBook, MeasurementBookResponse>()
                .ForMember(m => m.Status, opt => opt.MapFrom(p => p.Status.ToString()));

            //CreateMap<MBookItem, MBookItemResponse>()
            //    .ForMember(m => m.ItemNo, opt => opt.MapFrom(p => p.WorkOrderItem.ItemNo))
            //    .ForMember(m => m.PoQuantity, opt => opt.MapFrom(p => p.WorkOrderItem.PoQuantity))
            //    .ForMember(m => m.Description, opt => opt.MapFrom(p => p.WorkOrderItem.Description))
            //    .ForMember(m => m.UnitRate, opt => opt.MapFrom(p => p.WorkOrderItem.UnitRate))
            //    .ForMember(m => m.Uom, opt => opt.MapFrom(p => p.WorkOrderItem.Uom));
        }
    }
}
