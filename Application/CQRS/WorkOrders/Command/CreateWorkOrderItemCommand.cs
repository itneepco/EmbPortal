using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record CreateWorkOrderItemCommand(int workOrderId, WorkOrderItemRequest data) : IRequest<int>
    {
    }

    public class CreateWorkOrderItemCommandHandler : IRequestHandler<CreateWorkOrderItemCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateWorkOrderItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateWorkOrderItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.workOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.workOrderId);
            }

            workOrder.AddUpdateLineItem(
                description: request.data.Description,
                itemNo: request.data.ItemNo,
                uomId: request.data.UomId,
                rate: request.data.UnitRate,
                poQuantity: request.data.PoQuantity
            );

            await _context.SaveChangesAsync(cancellationToken);

            var item = workOrder.Items.FirstOrDefault(p => p.ItemNo == request.data.ItemNo);
            return item.Id;
        }
    }

}
