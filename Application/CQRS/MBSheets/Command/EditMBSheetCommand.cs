using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
                throw new NotFoundException(nameof(MBSheet), request.MBSheetID);
            }

            mbSheet.SetMeasurementDate((DateTime)request.Data.MeasurementDate);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
