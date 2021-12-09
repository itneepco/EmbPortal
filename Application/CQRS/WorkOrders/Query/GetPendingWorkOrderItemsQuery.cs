using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Query
{
    public record GetPendingWorkOrderItemsQuery(int workOrderId) : IRequest<IReadOnlyList<PendingOrderItemResponse>>
    {
    }

    public class GetPendingWorkOrderItemsQueryHandler : IRequestHandler<GetPendingWorkOrderItemsQuery, IReadOnlyList<PendingOrderItemResponse>>
    {
        private readonly IAppDbContext _context;
        public GetPendingWorkOrderItemsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PendingOrderItemResponse>> Handle(GetPendingWorkOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                    .ThenInclude(i => i.MBookItem)
                .FirstOrDefaultAsync(p => p.Id == request.workOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.workOrderId);
            }

            List<PendingOrderItemResponse> response = new();

            foreach (var item in workOrder.Items)
            {
                response.Add(new PendingOrderItemResponse
                {
                    ItemId = item.Id,
                    Description = item.Description,
                    IsPending = item.MBookItem == null ? true : false
                }); ;
            }

            return response.AsReadOnly();
        }
    }
}
