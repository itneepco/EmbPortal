using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record DeleteMBSheetCommand(int Id) : IRequest
    {
    }

    public class DeleteMBSheetCommandHandler : IRequestHandler<DeleteMBSheetCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteMBSheetCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(mbSheet), request.Id);
            }

            if (mbSheet.Status != MBSheetStatus.CREATED)
            {
                throw new BadRequestException("Published measurement book cannot be deleted");
            }

            _context.MBSheets.Remove(mbSheet);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
