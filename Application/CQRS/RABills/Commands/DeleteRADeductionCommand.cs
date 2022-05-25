using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record DeleteRADeductionCommand(int RABillId, int RADeductionId) : IRequest
    {
    }

    public class DeleteRADeductionCommandHandler : IRequestHandler<DeleteRADeductionCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteRADeductionCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRADeductionCommand request, CancellationToken cancellationToken)
        {
            var raBill = await _context.RABills
                .Include(p => p.Deductions)
                .Where(p => p.Id == request.RABillId)
                .FirstOrDefaultAsync();

            if (raBill == null)
            {
                throw new NotFoundException(nameof(RABill), request.RABillId);
            }

            if (raBill.Status == RABillStatus.APPROVED)
            {
                throw new BadRequestException("RA Bill has already been approved");
            }

            var deduction = raBill.Deductions.FirstOrDefault(p => p.Id == request.RADeductionId);

            if (deduction == null)
            {
                throw new NotFoundException(nameof(RADeduction), request.RADeductionId);
            }

            raBill.RemoveDeduction(deduction);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
