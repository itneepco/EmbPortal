using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record CreateRADeductionCommand(RADeductionRequest Data, int RABillId) : IRequest<int>
    {
    }

    public class CreateRADeductionCommandHandler : IRequestHandler<CreateRADeductionCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateRADeductionCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateRADeductionCommand request, CancellationToken cancellationToken)
        {
            var raBill = await _context.RABills
                .Include(p => p.Deductions)
                .Where(p => p.Id == request.RABillId)
                .FirstOrDefaultAsync();

            if (raBill == null)
            {
                throw new NotFoundException(nameof(raBill), request.RABillId);
            }

            if (raBill.Status == RABillStatus.APPROVED)
            {
                throw new BadRequestException("RA Bill has already been approved");
            }

            raBill.AddDeduction(new RADeduction(request.Data.Description, request.Data.Amount));

            await _context.SaveChangesAsync(cancellationToken);

            var deduction = raBill.Deductions.Last();

            return deduction.Id;
        }
    }
}
