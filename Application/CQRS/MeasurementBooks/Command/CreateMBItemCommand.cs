using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record CreateMBItemCommand(int mBookId, int wOrderItemId) : IRequest<int>
    {
    }

    public class CreateMBItemCommandHandler : IRequestHandler<CreateMBItemCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly IWorkOrderService _orderService;

        public CreateMBItemCommandHandler(IAppDbContext context, IWorkOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<int> Handle(CreateMBItemCommand req, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == req.mBookId);

            if (measurementBook == null)
            {
                throw new NotFoundException(nameof(measurementBook), req.mBookId);
            }

            //var workOrder = await _orderService.GetWorkOrderWithItems(measurementBook.WorkOrderId);
            //var workOrderItem = workOrder.Items.FirstOrDefault(p => p.Id == req.wOrderItemId);

            //if (workOrderItem == null)
            //{
            //    throw new NotFoundException($"WorkOrder does not have LineItem with Id: {req.wOrderItemId}");
            //}
            //if (workOrderItem.MBookItem != null)
            //{
            //    throw new BadRequestException($"LineItem with Id: {req.wOrderItemId} is being used in some other Measurement Book");
            //}

            measurementBook.AddUpdateLineItem(req.wOrderItemId);
            await _context.SaveChangesAsync(cancellationToken);

            var item = measurementBook.Items.FirstOrDefault(p => p.WorkOrderItemId == req.wOrderItemId);
            return item.Id;
        }
    }

}
