using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record EditMBItemCommand(int id, int mBookId, int wOrderItemId) : IRequest
    {
    }

    public class EditMBItemCommandHandler : IRequestHandler<EditMBItemCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IWorkOrderService _orderService;

        public EditMBItemCommandHandler(IAppDbContext context, IWorkOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<Unit> Handle(EditMBItemCommand req, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == req.mBookId);
            if (measurementBook == null)
            {
                throw new NotFoundException(nameof(measurementBook), req.mBookId);
            }

            var workOrder = await _orderService.GetWorkOrderWithItems(measurementBook.WorkOrderId);
            var workOrderItem = workOrder.Items.FirstOrDefault(p => p.Id == req.wOrderItemId);

            if (workOrderItem == null)
            {
                throw new NotFoundException($"WorkOrder does not have LineItem with Id: {req.wOrderItemId}");
            }
            if (workOrderItem.MBookItem != null)
            {
                throw new BadRequestException($"LineItem with Id: {req.wOrderItemId} is being used in some other Measurement Book");
            }

            measurementBook.AddUpdateLineItem(req.wOrderItemId);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
