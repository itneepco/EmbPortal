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
    public record CreateMBSheetCommand(MBSheetRequest data) : IRequest<int>
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
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                      .ThenInclude(i => i.Uom)
                 .Where(p => p.Id == request.data.MeasurementBookId)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

            if (mBook == null)
            {
                throw new NotFoundException($"MeasurementBook does not exist with Id: {request.data.MeasurementBookId}");
            }

            var mbSheet = new MBSheet
            (
               measurementBookId: request.data.MeasurementBookId,
               measurementOfficer: mBook.MeasurementOfficer,
               measurementDate: (DateTime)request.data.MeasurementDate,
               validationOfficer: mBook.ValidatingOfficer,
               acceptingOfficer: mBook.WorkOrder.EngineerInCharge
            );

            foreach (var item in request.data.Items)
            {
                MBookItem mBookItem = mBook.Items.FirstOrDefault(p => p.Id == item.MBookItemId);
                
                if (mBookItem == null)
                {
                    throw new NotFoundException($"MeasurementBook does not have line item with Id: {item.MBookItemId}");
                }

                mbSheet.AddLineItem(new MBSheetItem
                (
                    description: mBookItem.WorkOrderItem.Description,
                    uom: mBookItem.WorkOrderItem.Uom.Description,
                    dimension: ((int)mBookItem.WorkOrderItem.Uom.Dimension),
                    mBBookItemId: request.data.MeasurementBookId,
                    value1: item.Value1,
                    value2: item.Value2,
                    value3: item.Value3
                ));
            }

            _context.MBSheets.Add(mbSheet);
            await _context.SaveChangesAsync(cancellationToken);
            return mbSheet.Id;
        }
    }
}
