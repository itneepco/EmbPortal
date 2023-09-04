using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command;
public record UpdateWorkOrderCommand(PurchaseOrder PurchaseOrder) : IRequest<int>
{
}
public class UpdateWorkOrderCommandCommandHandler : IRequestHandler<UpdateWorkOrderCommand,int>
{
    private readonly IAppDbContext _context;
    private readonly IWorkOrderService _orderService;
    public UpdateWorkOrderCommandCommandHandler(IAppDbContext context, IWorkOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }
    public async Task<int> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var workOrder = await _context.WorkOrders
            .Include(order => order.Items)
            .FirstOrDefaultAsync(p => p.OrderNo == request.PurchaseOrder.OrderNo);
        
        if (workOrder is null)
        {
            throw new BadRequestException($"Work order No. {request.PurchaseOrder.OrderNo} doest not exist in EMB");
        }

        foreach (var item in request.PurchaseOrder.Items.Where(p => !bool.Parse(p.IsDeleted)))
        {
            foreach (var subItem in item.Details.Where(p => !bool.Parse(p.IsDeleted)))
            {
               
                var exist = workOrder.Items.FirstOrDefault(i => i.SubItemNo == subItem.SubItemNo && i.ItemNo == item.ItemNo);
                if (exist != null) 
                    continue; 
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
                Console.WriteLine($"Sub Item No : {subItem.SubItemNo}");
            }
        }
      
        await _context.SaveChangesAsync(cancellationToken);       
        return workOrder.Id;
    }
    
}
