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

            _context.MBSheets.Add(mbSheet);
            await _context.SaveChangesAsync(cancellationToken);
            return mbSheet.Id;
        }
    }
}
