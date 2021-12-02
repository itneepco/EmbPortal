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
    public record CreateMBItemCommand(int mBookId, MBookItemRequest data) : IRequest<int>
    {
    }

    public class CreateMBItemCommandHandler : IRequestHandler<CreateMBItemCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly IWorkOrderItemService _itemService;

        public CreateMBItemCommandHandler(IAppDbContext context, IWorkOrderItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }

        public async Task<int> Handle(CreateMBItemCommand req, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == req.mBookId);
            
            if (measurementBook == null) throw new NotFoundException(nameof(measurementBook), req.mBookId);

            var isAvailable = await _itemService.IsBalanceQtyAvailable(req.data.WorkOrderItemId, req.data.Quantity);

            if (!isAvailable)
            {
                throw new BadRequestException("Insufficient balance quantity for item " + req.data.ItemNo);
            }

            measurementBook.AddUpdateLineItem(req.data.WorkOrderItemId, req.data.Quantity);

            await _context.SaveChangesAsync(cancellationToken);

            var item = measurementBook.Items.FirstOrDefault(p => p.WorkOrderItemId == req.data.WorkOrderItemId);
            return item.Id;
        }
    }

}
