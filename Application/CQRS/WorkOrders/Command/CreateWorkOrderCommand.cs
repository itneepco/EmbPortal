using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using Infrastructure.Interfaces;
using MediatR;
using System;
using EmbPortal.Shared.Responses;
using System.Linq;
using Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Enums;

namespace Application.WorkOrders.Command;

public record CreateWorkOrderCommand(PurchaseOrder PurchaseOrder) : IRequest<int>
{
}

public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkOrderCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
       _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var workOrderDb = await _context.WorkOrders.FirstOrDefaultAsync(p => p.OrderNo == request.PurchaseOrder.OrderNo);
        if (workOrderDb != null)
        {
            throw new BadRequestException("Purchase order already exists in the database");
        }

        var workOrder = new WorkOrder
        {
            OrderNo = request.PurchaseOrder.OrderNo,
            OrderDate = request.PurchaseOrder.OrderDate,
            Project = request.PurchaseOrder.ProjectName,
            Contractor = request.PurchaseOrder.ContractorName,
            EngineerInCharge = _currentUserService.EmployeeCode,
            Status = WorkOrderStatus.CREATED
        }; 

        foreach (var item in request.PurchaseOrder.Items.Where(p => !bool.Parse(p.IsDeleted)))
        {
            foreach (var subItem in item.Details.Where(p => !bool.Parse(p.IsDeleted)))
            {
                
                workOrder.AddUpdateLineItem(
                    itemNo: item.ItemNo,
                    pacakageNo: item.PackageNo,
                    itemDesc: item.Description,
                    subItemNo: subItem.SubItemNo,
                    subItemPacakageNo: subItem.SubItemPackageNo,
                    serviceNo: subItem.ServiceNo,
                    shortServiceDesc: subItem.ShortDesc,
                    longServiceDesc: subItem.LongDesc,
                    uom: subItem.Uom,
                    unitRate: subItem.UnitRate,
                    poQuantity: subItem.Quantity
                );
            }
        }

        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync(cancellationToken);

        return workOrder.Id;


        throw new NotImplementedException();
    }
}