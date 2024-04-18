using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Queries;

public record GetRABillByIdQuery(int Id) : IRequest<RABillDetailResponse>
{
}

public class GetRABillByIdQueryHandler : IRequestHandler<GetRABillByIdQuery, RABillDetailResponse>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetRABillByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RABillDetailResponse> Handle(GetRABillByIdQuery request, CancellationToken cancellationToken)
    {
        var raBillQuery = _context.RABills
            .Include(p => p.Items)
            .Include(p => p.Deductions)
            .AsQueryable();

        var woQuery = _context.WorkOrders
            .Include(p => p.Items)                
            .AsQueryable();

        var query = from raBill in raBillQuery
                    join workOrder in woQuery on raBill.WorkOrderId equals workOrder.Id
                    select new { raBill, workOrder };

        var result = await query.FirstOrDefaultAsync(p => p.raBill.Id == request.Id);

        if (result == null)
        {
            throw new NotFoundException(nameof(result), request.Id);
        }

        var raBillResponse = _mapper.Map<RABillDetailResponse>(result.raBill);
        raBillResponse.WorkOrderNo = result.workOrder.OrderNo.ToString();
        foreach (var item in raBillResponse.Items)
        {
            var woItem = result.workOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);

            if(woItem == null)
            {
                throw new NotFoundException("Work order line item not found!");
            }
            item.PoQuantity = woItem.PoQuantity;
            item.UnitRate = woItem.UnitRate;
            item.SubItemNo = woItem.SubItemNo;
            item.ServiceNo = woItem.ServiceNo;
            item.ItemDescription = woItem.ItemDescription;
            item.ShortServiceDesc = woItem.ShortServiceDesc;
        }

        return raBillResponse;
    }
}
