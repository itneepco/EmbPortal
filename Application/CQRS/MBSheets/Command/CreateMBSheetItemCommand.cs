using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record CreateMBSheetItemCommand(int MBSheetId, MBSheetItemRequest Data) : IRequest<int>
    {
    }

    public class CreateMBSheetItemCommandHandler : IRequestHandler<CreateMBSheetItemCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateMBSheetItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateMBSheetItemCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                .Where(p => p.Id == request.MBSheetId)
                .FirstOrDefaultAsync();

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.MBSheetId);
            }

            MeasurementBook mBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                      .ThenInclude(i => i.Uom)
                .Where(p => p.Id == mbSheet.MeasurementBookId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (mBook == null)
            {
                throw new NotFoundException(nameof(MeasurementBook), mbSheet.MeasurementBookId);
            }

            MBookItem mBookItem = mBook.Items.FirstOrDefault(p => p.Id == request.Data.MBookItemId);

            if (mBookItem == null)
            {
                throw new NotFoundException($"Measurement Book does not have line item with Id: {request.Data.MBookItemId}");
            }

            mbSheet.AddLineItem(new MBSheetItem
            (
                // fill up the data from measurement book item details --- START ---
                uom: mBookItem.WorkOrderItem.Uom.Name,
                dimension: ((int)mBookItem.WorkOrderItem.Uom.Dimension),
                rate: mBookItem.WorkOrderItem.UnitRate,
                mbItemDescription: mBookItem.WorkOrderItem.Description,
                mBookItemId: mBookItem.Id,
                // --- END ---

                // fill up the data from the request object coming from client --- START ---
                description: request.Data.Description,
                nos: request.Data.Nos,
                value1: request.Data.Value1,
                value2: request.Data.Value2,
                value3: request.Data.Value3
                // --- END ---
            ));

            await _context.SaveChangesAsync(cancellationToken);

            var item = mbSheet.Items.Last();

            return item.Id;
        }
    }
}
