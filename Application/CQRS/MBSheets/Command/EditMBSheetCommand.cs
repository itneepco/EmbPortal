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
    public record EditMBSheetCommand(MBSheetRequest Data, int MBSheetID) : IRequest
    {
    }

    public class EditMBSheetCommandHandler : IRequestHandler<EditMBSheetCommand>
    {
        private readonly IAppDbContext _context;

        public EditMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditMBSheetCommand request, CancellationToken cancellationToken)
        {
            MBSheet mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.MBSheetID);

            if (mbSheet == null)
            {
                throw new NotFoundException($"Measurement Sheet does not exist with Id: {request.MBSheetID}");
            }

            mbSheet.SetMeasurementDate((DateTime)request.Data.MeasurementDate);
            mbSheet.SetTitle(request.Data.Title);

            // iterate over all mbsheet items and remove items that are not in request object
            //foreach (var item in mbSheet.Items)
            //{
            //    var itemInRequest = request.Data.Items.Find(p => p.Id == item.Id);
            //    if (itemInRequest == null)
            //    {
            //        mbSheet.RemoveLineItem(item);
            //    }
            //}

            MeasurementBook mBook = await _context.MeasurementBooks
                .Include(p => p.WorkOrder)
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                      .ThenInclude(oi => oi.Uom)
                .Where(p => p.Id == mbSheet.MeasurementBookId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (mBook == null)
            {
                throw new NotFoundException($"MeasurementBook does not exist with Id: {request.Data.MeasurementBookId}");
            }

            // iterate over all items in request object and update the mbsheet items
            foreach (var item in request.Data.Items)
            {
                // if the item total quantity is less than or equal to zero, raise exception
                if (item.Total <= 0)
                {
                    throw new BadRequestException("Item quantity cannot be less than or equal to Zero");
                }

                // Check if the item already exists in the database
                var itemInDb = mbSheet.Items.FirstOrDefault(p => p.Id == item.Id);
                if(itemInDb != null) // if item exist, update item accordingly
                {
                    itemInDb.SetDescription(item.Description);
                    itemInDb.SetNos(item.Nos);
                    itemInDb.SetValue1(item.Value1);
                    itemInDb.SetValue2(item.Value2);
                    itemInDb.SetValue3(item.Value3);
                }
                else // if item is not in database, add item to the database
                {
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
                        mBookItemId: mBookItem.Id,
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
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
