using Application.Interfaces;
using EmbPortal.Shared.Responses.RA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Queries;

public record GetRAByWorkOrder(int WorkOrderId) : IRequest<List<RAResponse>>
{
}

public class GetRAByWorkOrderQueryHandler : IRequestHandler<GetRAByWorkOrder, List<RAResponse>> {
    private readonly IAppDbContext _db;
    public GetRAByWorkOrderQueryHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<List<RAResponse>> Handle(GetRAByWorkOrder request, CancellationToken cancellationToken)
    {
        var raBills = await _db.RAHeaders
                            .Include(x => x.Items)
                            .Include(y => y.Deductions)
                            .Where(p => p.WorkOrderId == request.WorkOrderId)
                            .Select(q => new RAResponse
                            {
                                Id = q.Id,
                                Title = q.Title,
                                Status = q.Status,
                                BillDate = q.BillDate,
                                RAAmount = q.Items.Sum(i => i.UnitRate * (decimal)i.CurrentRAQty),
                                Deduction = q.Deductions.Sum(d => d.Amount)
                            }).
                            ToListAsync(cancellationToken);
        return raBills;
    }
}
