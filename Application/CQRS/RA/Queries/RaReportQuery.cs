using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.RAAggregate;
using EmbPortal.Shared.Responses.RA;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Queries;

public record RaReportQuery(int id) : IRequest<RaReportView>
{
}

public class RaReportQueryHandler : IRequestHandler<RaReportQuery, RaReportView>
{
    private readonly IAppDbContext _db;

    public RaReportQueryHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<RaReportView> Handle(RaReportQuery request, CancellationToken cancellationToken)
    {
        var raQuery = _db.RAHeaders
            .Include(i => i.Items)
            .Include(i => i.Deductions)
            .AsQueryable();

        var wOrderQuery = _db.WorkOrders
            .Include(i => i.Items)
            .AsQueryable();

        var query = from ra in raQuery
                     join worder in wOrderQuery on ra.WorkOrderId equals worder.Id
                     select new { ra, worder };

        var result = await query.FirstOrDefaultAsync(p => p.ra.Id == request.id);
        if (result == null)
        {
            throw new NotFoundException(nameof(result), request.id);
        }

        var report = new RaReportView
        {
            RaTitle = result.ra.Title,
            PoNo = result.worder.OrderNo,
            Contractor = result.worder.Contractor,
            Eic = result.worder.EngineerInCharge,          
            BillDate = result.ra.BillDate,
            FromDate = result.ra.FromDate,
            ToDate = result.ra.ToDate,
            LastBill = result.ra.LastBillDetail,
            Remarks = result.ra.Remarks,
            TotalRaAmount = result.ra.Items.Sum(i => i.UnitRate * (decimal)i.CurrentRAQty),
            TotalDeduction = result.ra.Deductions.Sum(d => d.Amount),
            Items = result.ra.Items.GroupBy( p=> new {p.ItemNo,p.ItemDescription})
                .Select(i => new Item
                {
                    No = i.Key.ItemNo,
                    Description = i.Key.ItemDescription,
                    SubItems = i.Select( p => new SubItem
                    {
                        No = p.SubItemNo,
                        ShortServiceDesc = p.ShortServiceDesc,
                        Uom = p.Uom,
                        UnitRate = p.UnitRate,
                        PoQuantity = p.PoQuantity,
                        MeasuredQty = p.MeasuredQty,
                        TillLastRaQty = p.TillLastRAQty,
                        CurrentRaQty = p.CurrentRAQty,
                        Remarks = p.Remarks
                    }).ToList()
                }).ToList(),
            Deductions = result.ra.Deductions.Select(d => new DeductionView
            {
                Description = d.Description,
                Amount = d.Amount,
            }).ToList()
        };        
        
        return report;
    }
}