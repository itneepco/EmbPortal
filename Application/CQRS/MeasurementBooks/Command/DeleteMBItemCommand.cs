using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record DeleteMBItemCommand(int id, int mBookId) : IRequest
    {
    }

    public record DeleteMBItemCommandHandler : IRequestHandler<DeleteMBItemCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteMBItemCommandHandler(IAppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Unit> Handle(DeleteMBItemCommand request, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.mBookId);

            if (measurementBook == null)
            {
                throw new NotFoundException(nameof(measurementBook), request.mBookId);
            }

            measurementBook.RemoveLineItem(request.id);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
