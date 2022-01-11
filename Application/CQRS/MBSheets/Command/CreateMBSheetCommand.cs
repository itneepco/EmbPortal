using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record CreateMBSheetCommand(CreateMBSheetRequest data) : IRequest<int>
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

            var mbSheet = new MBSheet
           (
               measurementBookId: request.data.MeasurementBookId,
               measurementOfficer: request.data.MeasurementOfficer,
               measurementDate: request.data.MeasurementDate,
               validationOfficer: request.data.ValidationOfficer,
               acceptingOfficer: request.data.AcceptingOfficer
               
           ); 
            

            MeasurementBook mBBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                      .ThenInclude(i => i.Uom)
                 .Where(p => p.Id == request.data.MeasurementBookId)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();
            if (mBBook == null)
            {
                throw new NotFoundException($"MBBook does not exist with Id: {request.data.MeasurementBookId}");
            }
            
                IReadOnlyList<MBookItem> mBookItems = mBBook.Items;
                foreach (var item in mBookItems)
                {
                    mbSheet.AddLineItem(new MBSheetItem
                        (
                          description: item.WorkOrderItem.Description,
                          uom: item.WorkOrderItem.Uom.Description,
                          dimension: ((int)item.WorkOrderItem.Uom.Dimension),
                          mBBookItemId: request.data.MeasurementBookId
                        )) ;

                }
                _context.MBSheets.Add(mbSheet);
                await _context.SaveChangesAsync(cancellationToken);
                return mbSheet.Id;            
        }
    }
}
