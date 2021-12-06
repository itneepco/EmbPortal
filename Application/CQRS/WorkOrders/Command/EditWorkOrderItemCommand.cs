using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record EditWorkOrderItemCommand(int id, int workOrderId, WorkOrderItemRequest data) : IRequest
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
                .FirstOrDefaultAsync(p => p.Id == request.workOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.workOrderId);
            }

            List<SubItem> subItems = new();
            foreach (var item in request.data.SubItems)
            {
                subItems.Add(new SubItem(item.Description, item.UomId, item.UnitRate, item.PoQuantity));
            }

            workOrder.AddUpdateLineItem(
                id: request.id,
                description: request.data.Description,
                subItems: subItems
            );

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
