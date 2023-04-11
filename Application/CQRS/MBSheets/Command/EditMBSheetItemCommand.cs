using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record EditMBSheetItemCommand(int Id, int MBSheetId, MBSheetItemRequest Data) : IRequest
    {
    }

    public class EditMBSheetItemCommandHandler : IRequestHandler<EditMBSheetItemCommand>
    {
        private readonly IAppDbContext _context;

        public EditMBSheetItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditMBSheetItemCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                .Where(p => p.Id == request.MBSheetId)
                .FirstOrDefaultAsync();

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.MBSheetId);
            }

            var mbSheetItem = mbSheet.Items.FirstOrDefault(p => p.Id == request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException($"Current MB Sheet does not have line item with Id: {request.Id}");
            }

            mbSheetItem.SetDescription(request.Data.Description);
            mbSheetItem.SetNos(request.Data.Nos);
            mbSheetItem.SetMeasuredQuantity(request.Data.MeasuredQuantity);           

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
