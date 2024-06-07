using Application.Interfaces;
using Domain.Entities.RAAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests.RA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Commands;

public record CreateRACommand(RARequest data) : IRequest<int>
{
}

public class CreateRACommandHandler : IRequestHandler<CreateRACommand, int>
{
    private readonly IAppDbContext _db;

    public CreateRACommandHandler(IAppDbContext db)
    {
        _db= db;
    }

    public async  Task<int> Handle(CreateRACommand request, CancellationToken cancellationToken)
    {
       
        var worder = await _db.WorkOrders.SingleAsync(p => p.Id == request.data.WorkOrderId);
                
        var raBillCount = _db.RAHeaders.Count(i => i.WorkOrderId == worder.Id) + 1;
        var raTitle = worder!.OrderNo + "-RA-" + raBillCount;
        var raBill = new RAHeader
        {
            Title = raTitle,
            BillDate = (DateTime)request.data.BillDate,
            FromDate = (DateTime)request.data.FromDate,
            ToDate = (DateTime)request.data.ToDate,
            CompletionDate = request.data.CompletionDate == null ? null: (DateTime)request.data.CompletionDate,
            LastBillDetail = request.data.LastBillDetail,
            EicEmpCode = worder.EngineerInCharge,
            Status = RAStatus.Draft,
            WorkOrderId = request.data.WorkOrderId,
            Remarks = request.data.Remarks
        };
        foreach (var item in request.data.Items)
        {
            if(item.CurrentRAQty > 0)
            {
                raBill.AddItem(
                     new RAItem
                     {
                         WorkOrderItemId = item.WorkOrderItemId,
                         ItemNo = item.ItemNo,
                         ItemDescription = item.ItemDescription,
                         PackageNo = item.PackageNo,
                         SubItemNo = item.SubItemNo,
                         SubItemPackageNo = item.SubItemPackageNo,
                         ServiceNo = item.ServiceNo,
                         ShortServiceDesc = item.ShortServiceDesc,
                         UnitRate = item.UnitRate,
                         Uom = item.Uom,
                         PoQuantity = item.PoQuantity,
                         MeasuredQty = item.MeasuredQty,
                         TillLastRAQty = item.TillLastRAQty,
                         CurrentRAQty = item.CurrentRAQty
                     }
                 );
            }
                     
        }
        foreach (var item in request.data.Deductions)
        {
            raBill.AddDeduction(new Deduction
            {
                Description = item.Description,
                Amount = item.Amount               
            });
        }
        _db.RAHeaders.Add(raBill);
        await _db.SaveChangesAsync(cancellationToken);
        return raBill.Id;

    }
}
