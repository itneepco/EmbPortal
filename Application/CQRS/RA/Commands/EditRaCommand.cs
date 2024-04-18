using Application.Interfaces;
using Domain.Entities.RAAggregate;
using EmbPortal.Shared.Requests.RA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Commands;

public record EditRaCommand(RARequest data, int id) : IRequest
{
    
}

public class EditRaHandler : IRequestHandler<EditRaCommand>
{
    private readonly IAppDbContext _db;

    public EditRaHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Unit> Handle(EditRaCommand request, CancellationToken cancellationToken)
    {
        var ra = await _db.RAHeaders
                    .Include(p => p.Items)
                    .Include(p => p.Deductions)
                    .SingleOrDefaultAsync(i => i.Id == request.id);

        //var worder = await _db.WorkOrders.Include(w => w.Items)
        //               .SingleAsync(p => p.Id == request.data.WorkOrderId);

        ra.BillDate = (DateTime)request.data.BillDate;
        ra.FromDate = (DateTime)request.data.FromDate;
        ra.ToDate = (DateTime)request.data.ToDate;
        ra.CompletionDate = (request.data.CompletionDate != null )? (DateTime)request.data.CompletionDate : null;
        ra.LastBillDetail = request.data.LastBillDetail;
        ra.Remarks = request.data.Remarks;
        foreach (var item in ra.Items)
        {

            var newItem = request.data.Items.Single(p => p.WorkOrderItemId == item.WorkOrderItemId);            
            //worder.UpdateRaQuantity(item.CurrentRAQty, newItem.CurrentRAQty, item.WorkOrderItemId);
            item.CurrentRAQty = newItem.CurrentRAQty;
        }
        ra.RemoveAllDeductions();
        foreach(var deduction in request.data.Deductions)
        {
            ra.AddDeduction(new Deduction
            {
                Description = deduction.Description,
                Amount = deduction.Amount
            });
        }
        await _db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}