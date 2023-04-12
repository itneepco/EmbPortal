using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record DeleteWorkOrderItemCommand(int Id, int WorkOrderId) : IRequest
    {
    }

    public class DeleteWorkOrderItemCommandHandler : IRequestHandler<DeleteWorkOrderItemCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteWorkOrderItemCommandHandler(IAppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Unit> Handle(DeleteWorkOrderItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.WorkOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(WorkOrder), request.WorkOrderId);
            }

            var orderItem = workOrder.Items.FirstOrDefault(p => p.Id == request.Id);

            if (orderItem == null)
            {
                throw new NotFoundException(nameof(WorkOrderItem), request.Id);
            }

            

            workOrder.RemoveLineItem(request.Id);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
