using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.MBSheetAggregate;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.RABillAggregate;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Responses;
using EmbPortal.Shared.Responses.MBSheets;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserResponse>()
                .ForMember(m => m.EmployeeCode, opt => opt.MapFrom(p => p.UserName));            

            CreateMap<WorkOrder, WorkOrderResponse>()
                .ForMember(m => m.EicEmployeeCode, opt => opt.MapFrom(p => p.EngineerInCharge));

            CreateMap<WorkOrder, WorkOrderDetailResponse>();

            CreateMap<WorkOrderItem, WorkOrderItemResponse>();    

            CreateMap<MeasurementBook, MBookResponse>();

            CreateMap<MBookItem, MBookItemResponse>();

            CreateMap<MeasurementBook, MBookHeaderResponse>();

            CreateMap<MBSheet, MBSheetResponse>();

            CreateMap<MBSheet, MBSheetInfoResponse>();

            CreateMap<MBSheetItem, MBSheetItemResponse>();

            CreateMap<MBItemMeasurement, MBSheetItemMeasurementResponse>();

            CreateMap<ItemAttachment, ItemAttachmentResponse>();

            CreateMap<RABill, RABillResponse>();

            CreateMap<RABill, RABillDetailResponse>();

            CreateMap<RABill, RABillInfoResponse>()
                //.ForMember(m => m.OrderNo, opt => opt.MapFrom(p => p.MeasurementBook.WorkOrder.OrderNo))
                //.ForMember(m => m.OrderDate, opt => opt.MapFrom(p => p.MeasurementBook.WorkOrder.OrderDate))
                //.ForMember(m => m.MBookTitle, opt => opt.MapFrom(p => p.MeasurementBook.Title))
                .ForMember(m => m.RABillTitle, opt => opt.MapFrom(p => p.Title));

            CreateMap<RABillItem, RABillItemResponse>();

            CreateMap<RADeduction, RADeductionResponse>();
        }
    }
}
