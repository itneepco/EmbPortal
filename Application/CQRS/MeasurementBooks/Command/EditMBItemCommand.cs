using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record EditMBItemCommand(int id, int mBookId, MBookItemRequest data) : IRequest
    {
    }

    public class EditMBItemCommandHandler : IRequestHandler<EditMBItemCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IWorkOrderItemService _itemService;

        public EditMBItemCommandHandler(IAppDbContext context, IWorkOrderItemService itemService)
        {
            _context = context;
            _itemService = itemService;
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

            var isAvailable = await _itemService.IsBalanceQtyAvailable(req.data.WorkOrderItemId, req.data.Quantity);

            if (!isAvailable)
            {
                throw new BadRequestException("Insufficient balance quantity for item " + req.data.ItemNo);
            }

            measurementBook.AddUpdateLineItem(req.data.WorkOrderItemId, req.data.Quantity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
