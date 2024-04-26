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

        public async Task Handle(EditMBSheetItemCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets
                .Include(p => p.Items).ThenInclude(p => p.Measurements)
                .Where(p => p.Id == request.MBSheetId)
                .FirstOrDefaultAsync();

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.MBSheetId);
            }

            var mbSheetItem = mbSheet.Items.FirstOrDefault(p => p.Id == request.Id);

            if (mbSheetItem == null)
            {
                throw new NotFoundException($"Current MB Sheet does not have line item with Id: {request.Id}");
            }

            mbSheetItem.ClearMeasurements();
            foreach (var measurement in request.Data.Measurements)
            {
                mbSheetItem.AddMeasurement(new MBItemMeasurement
                {
                    No = measurement.No,
                    Val1 = measurement.Val1,
                    Val2 = measurement.Val2,
                    Val3 = measurement.Val3,
                    Description = measurement.Description
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
