using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record PublishWorkOrderItemCommand(int WorkOrderId, int ItemId) : IRequest
    {
    }

    public class PublishWorkOrderItemCommandHandler : IRequestHandler<PublishWorkOrderItemCommand>
    {
        private readonly IAppDbContext _context;

        public PublishWorkOrderItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PublishWorkOrderItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.WorkOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(WorkOrder), request.WorkOrderId);
            }

            var orderItem = workOrder.Items.FirstOrDefault(p => p.Id == request.ItemId);

            if (orderItem == null)
            {
                throw new NotFoundException(nameof(WorkOrderItem), request.ItemId);
            }

            orderItem.MarkPublished();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
