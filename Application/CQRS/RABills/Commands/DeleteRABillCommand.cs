using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record DeleteRABillCommand(int Id) : IRequest
    {
    }

    public class DeleteRABillCommandHandler : IRequestHandler<DeleteRABillCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteRABillCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRABillCommand request, CancellationToken cancellationToken)
        {
            var raBill = await _context.RABills
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (raBill == null)
            {
                throw new NotFoundException(nameof(raBill), request.Id);
            }

            if (raBill.Status == RABillStatus.APPROVED)
            {
                throw new BadRequestException("Approved RA Bill cannot be deleted");
            }

            _context.RABills.Remove(raBill);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
