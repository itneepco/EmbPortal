using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Responses;
using EmbPortal.Shared.Responses.RABills;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Queries;

public record RABillReportQuery(int id) : IRequest<RABillReportResponse>
{
}

public class RABIllReportQueryHandler : IRequestHandler<RABillReportQuery, RABillReportResponse>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public RABIllReportQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RABillReportResponse> Handle(RABillReportQuery request, CancellationToken cancellationToken)
    {
        var raBillQuery = _context.RABills
            .Include(i => i.Items)
            .Include(i => i.Deductions)
            .AsQueryable();

        var wOrderQuery = _context.WorkOrders
            .Include(i => i.Items)
            .AsQueryable();

        var MBookQuery = _context.MeasurementBooks
            .AsQueryable();

        var query = from raBill in raBillQuery
                    join mBook in MBookQuery on raBill.MeasurementBookId equals mBook.Id
                    join wOrder in wOrderQuery on raBill.WorkOrderId equals wOrder.Id
                    select new {raBill,mBook, wOrder};

        var result = await query.FirstOrDefaultAsync(p => p.raBill.Id == request.id);

        if(result == null)
        {
            throw new NotFoundException(nameof(result), request.id);
        }
        var raBillItems = new List<RABillItemResponse>();

        foreach (var item in result.raBill.Items.Where(p => p.CurrentRAQty > 0))
        {
            var woItem = result.wOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);

            if (woItem == null)
            {
                throw new NotFoundException("Work order line item not found!");
            }
            var raBillItem = new RABillItemResponse
            {
                Id = item.Id,
                WorkOrderItemId = woItem.Id,
                PoQuantity = woItem.PoQuantity,
                UoM = woItem.Uom,
                ServiceNo = woItem.ServiceNo,
                ShortServiceDesc = woItem.ShortServiceDesc,
                UnitRate = woItem.UnitRate,
                AcceptedMeasuredQty = item.AcceptedMeasuredQty,
                TillLastRAQty = item.TillLastRAQty,
                CurrentRAQty = item.CurrentRAQty,
                Remarks = item.Remarks             
            };
           raBillItems.Add(raBillItem);           
        }
        var deductions = _mapper.Map<IReadOnlyList<RADeduction>, IReadOnlyList<RADeductionResponse>>(result.raBill.Deductions);
        var response = new RABillReportResponse
        {
            RABillTitle = result.raBill.Title,
            BillDate = result.raBill.BillDate,
            PoNo = result.wOrder.OrderNo.ToString(),
            MBTitle = result.mBook.Title,            
            EIC = result.mBook.EicEmpCode,
            MeaurementOfficer = result.mBook.MeasurerEmpCode,
            ValidationOfficer = result.mBook.ValidatorEmpCode,
            Contractor = result.wOrder.Contractor,
            RABillItems = raBillItems,
            Deductions = deductions
        };
            
        return response;
    }
}