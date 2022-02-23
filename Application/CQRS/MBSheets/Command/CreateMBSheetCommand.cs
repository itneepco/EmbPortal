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
    public record CreateMBSheetCommand(MBSheetRequest Data) : IRequest<int>
    {
    }

    public class CreateMBSheetCommandHandler : IRequestHandler<CreateMBSheetCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateMBSheetCommand request, CancellationToken cancellationToken)
        {
            MeasurementBook mBook = await _context.MeasurementBooks
                .Include(p => p.WorkOrder)
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                      .ThenInclude(i => i.Uom)
                .Where(p => p.Id == request.Data.MeasurementBookId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (mBook == null)
            {
                throw new NotFoundException($"MeasurementBook does not exist with Id: {request.Data.MeasurementBookId}");
            }

            var mbSheet = new MBSheet
            (
               title: request.Data.Title,
               measurementBookId: request.Data.MeasurementBookId,
               measurementOfficer: mBook.MeasurementOfficer,
               measurementDate: (DateTime)request.Data.MeasurementDate,
               validationOfficer: mBook.ValidatingOfficer,
               acceptingOfficer: mBook.WorkOrder.EngineerInCharge
            );

            foreach (var item in request.Data.Items)
            {
                // if the item total quantity is less than or equal to zero, raise exception
                if (item.Total <= 0)
                {
                    throw new BadRequestException("Item quantity cannot be less than or equal to Zero");
                }

                MBookItem mBookItem = mBook.Items.FirstOrDefault(p => p.Id == item.MBookItemId);
                
                if (mBookItem == null)
                {
                    throw new NotFoundException($"MeasurementBook does not have line item with Id: {item.MBookItemId}");
                }

                mbSheet.AddLineItem(new MBSheetItem
                (
                    // fill up the data from measurement book item details --- START ---
                    uom: mBookItem.WorkOrderItem.Uom.Name,
                    dimension: ((int)mBookItem.WorkOrderItem.Uom.Dimension),
                    rate: mBookItem.WorkOrderItem.UnitRate,
                    mbItemDescription: mBookItem.WorkOrderItem.Description,
                    mBookItemId: item.MBookItemId,
                    // --- END ---

                    // fill up the data from the request object coming from client --- START ---
                    description: item.Description,
                    nos: item.Nos,
                    value1: item.Value1,
                    value2: item.Value2,
                    value3: item.Value3
                    // --- END ---
                ));
            }

            _context.MBSheets.Add(mbSheet);
            await _context.SaveChangesAsync(cancellationToken);
            return mbSheet.Id;
        }
    }
}
