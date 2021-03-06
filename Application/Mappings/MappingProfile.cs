using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.MBSheetAggregate;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.RABillAggregate;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Responses;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserResponse>()
                .ForMember(m => m.EmployeeCode, opt => opt.MapFrom(p => p.UserName));

            CreateMap<Project, ProjectResponse>();

            CreateMap<Uom, UomResponse>()
                .ForMember(m => m.DimensionId, opt => opt.MapFrom(p => p.Dimension))
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Dimension.ToString()));

            CreateMap<Contractor, ContractorResponse>();

            CreateMap<WorkOrder, WorkOrderResponse>()
                .ForMember(m => m.ProjectName, opt => opt.MapFrom(p => p.Project.Name))
                .ForMember(m => m.ContractorName, opt => opt.MapFrom(p => p.Contractor.Name));

            CreateMap<WorkOrder, WorkOrderDetailResponse>()
                .ForMember(m => m.ProjectName, opt => opt.MapFrom(p => p.Project.Name))
                .ForMember(m => m.ContractorName, opt => opt.MapFrom(p => p.Contractor.Name));
                
            CreateMap<WorkOrderItem, WorkOrderItemResponse>()
                .ForMember(m => m.Uom, opt => opt.MapFrom(p => p.Uom.Name))
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Uom.Dimension));

            CreateMap<MeasurementBook, MBookResponse>();

            CreateMap<MBookItem, MBookItemResponse>()
                .ForMember(m => m.Description, opt => opt.MapFrom(p => p.WorkOrderItem.Description))
                .ForMember(m => m.Uom, opt => opt.MapFrom(p => p.WorkOrderItem.Uom.Name))
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.WorkOrderItem.Uom.Dimension))
                .ForMember(m => m.PoQuantity, opt => opt.MapFrom(p => p.WorkOrderItem.PoQuantity))
                .ForMember(m => m.UnitRate, opt => opt.MapFrom(p => p.WorkOrderItem.UnitRate));

            CreateMap<MeasurementBook, MBookHeaderResponse>()
                .ForMember(m => m.OrderNo, opt => opt.MapFrom(p => p.WorkOrder.OrderNo))
                .ForMember(m => m.OrderDate, opt => opt.MapFrom(p => p.WorkOrder.OrderDate))
                .ForMember(m => m.Contractor, opt => opt.MapFrom(p => p.WorkOrder.Contractor.Name));

            CreateMap<MeasurementBook, MBookDetailResponse>();

            CreateMap<MBSheet, MBSheetResponse>();

            CreateMap<MBSheetItem, MBSheetItemResponse>();

            CreateMap<ItemAttachment, ItemAttachmentResponse>();

            CreateMap<RABill, RABillResponse>();

            CreateMap<RABill, RABillDetailResponse>();

            CreateMap<RABill, RABillInfoResponse>()
                .ForMember(m => m.OrderNo, opt => opt.MapFrom(p => p.MeasurementBook.WorkOrder.OrderNo))
                .ForMember(m => m.OrderDate, opt => opt.MapFrom(p => p.MeasurementBook.WorkOrder.OrderDate))
                .ForMember(m => m.Contractor, opt => opt.MapFrom(p => p.MeasurementBook.WorkOrder.Contractor.Name))
                .ForMember(m => m.MBookTitle, opt => opt.MapFrom(p => p.MeasurementBook.Title))
                .ForMember(m => m.RABillTitle, opt => opt.MapFrom(p => p.Title));

            CreateMap<RABillItem, RABillItemResponse>()
                .ForMember(dest => dest.MBookItemDescription, opt => opt.MapFrom(src => src.ItemDescription));

            CreateMap<RADeduction, RADeductionResponse>();
        }
    }
}
