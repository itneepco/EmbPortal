using Application.Interfaces;
using EmbPortal.Shared.Responses.RA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Queries;

public record GetRAById(int id) : IRequest<RADetailResponse>
{
}

public class GetRAByIdQueryHandler : IRequestHandler<GetRAById, RADetailResponse>
{
    private readonly IAppDbContext _db;

    public GetRAByIdQueryHandler(IAppDbContext db)
    {
        _db = db;
    }
        
    public async Task<RADetailResponse> Handle(GetRAById request, CancellationToken cancellationToken)
    {
        
        var result = await _db.RAHeaders.
                        Include(p => p.Items).
                        Include(q => q.Deductions)                       
                        .Select(x => new RADetailResponse
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Status = x.Status,
                            BillDate = x.BillDate,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate,
                            CompletionDate = x.CompletionDate,
                            LastBillDetail = x.LastBillDetail,
                            Remarks = x.Remarks,
                            EicEmpCode = x.EicEmpCode,
                            TotalRaAmount = x.Items.Sum(i => i.UnitRate * (decimal)i.CurrentRAQty),
                            TotalDeduction = x.Deductions.Sum(d => d.Amount),
                            Items = x.Items.Select(a => new RaItemView
                            {
                                Id = a.Id,
                                WorkOrderItemId = a.WorkOrderItemId,
                                Uom = a.Uom,
                                UnitRate = a.UnitRate,
                                PoQuantity = a.PoQuantity,
                                MeasuredQty = a.MeasuredQty,
                                TillLastRaQty = a.TillLastRAQty,
                                CurrentRaQty = a.CurrentRAQty,
                                Remarks = a.Remarks,
                                ItemNo = a.ItemNo,
                                ItemDescription = a.ItemDescription,
                                SubItemNo = a.SubItemNo,
                                ServiceNo = a.ServiceNo,
                                ShortServiceDesc = a.ShortServiceDesc
                                
                            }).ToList(),

                            Deductions = x.Deductions.Select(b => new RaDeductionView
                            {
                                Id = b.Id,
                                Amount = b.Amount,
                                Description = b.Description
                            }).ToList()

                        }).
                        FirstOrDefaultAsync(r => r.Id == request.id);
        return result;
    }
}
