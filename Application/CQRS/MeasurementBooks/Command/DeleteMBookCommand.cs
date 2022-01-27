using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record DeleteMBookCommand(int id) : IRequest
    {
    }

    public class DeleteMBookCommandHandler : IRequestHandler<DeleteMBookCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteMBookCommandHandler(IAppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Unit> Handle(DeleteMBookCommand request, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks
                .FirstOrDefaultAsync(p => p.Id == request.id);

            if (measurementBook == null)
            {
                throw new NotFoundException(nameof(measurementBook), request.id);
            }

            if (measurementBook.Status == MBookStatus.PUBLISHED || measurementBook.Status == MBookStatus.COMPLETED)
            {
                throw new BadRequestException("Published measurement book cannot be deleted");
            }

            _context.MeasurementBooks.Remove(measurementBook);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
