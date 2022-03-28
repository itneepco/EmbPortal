using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Requests;
using System.Threading;
using System.Threading.Tasks;
using EmbPortal.Shared.Enums;
using System.Linq;
using Domain.Entities.WorkOrderAggregate;

namespace Application.CQRS.WorkOrders.Command
{
    public record EditWorkOrderItemCommand(int Id, int WorkOrderId, WorkOrderItemRequest Data) : IRequest
    {
    }

    public class EditWorkOrderItemCommandHandler : IRequestHandler<EditWorkOrderItemCommand>
    {
        private readonly IAppDbContext _context;

        public EditWorkOrderItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditWorkOrderItemCommand request, CancellationToken cancellationToken)
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

            if (orderItem.Status != WorkOrderItemStatus.PUBLISHED)
            {
                throw new BadRequestException("Published Work Order Item cannot be updated");
            }
            
            workOrder.AddUpdateLineItem(
                id: request.Id,
                description: request.Data.Description,
                uomId: request.Data.UomId,
                unitRate: request.Data.UnitRate,
                poQuantity: request.Data.PoQuantity
            );

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
