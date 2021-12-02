using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record EditMBItemCommand(int id, int workOrderId, WorkOrderItemRequest data) : IRequest
    {
    }

    public class EditWorkOrderItemCommandHandler : IRequestHandler<EditMBItemCommand>
    {
        private readonly IAppDbContext _context;

        public EditWorkOrderItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditMBItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.workOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.workOrderId);
            }

            workOrder.AddUpdateLineItem(
                id: request.id,
                description: request.data.Description,
                itemNo: request.data.ItemNo,
                uomId: request.data.UomId,
                rate: request.data.UnitRate,
                poQuantity: request.data.PoQuantity
            );

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
