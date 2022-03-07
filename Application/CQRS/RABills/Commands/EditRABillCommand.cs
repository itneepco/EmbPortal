using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record EditRABillCommand(int RaBillId, RABillRequest Data) : IRequest
    {
        
    }

    public class EditRABillCommandHandler : IRequestHandler<EditRABillCommand>
    {
        private readonly IAppDbContext _context;

        public EditRABillCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(EditRABillCommand request, CancellationToken cancellationToken)
        {
            RABill raBill = await _context.RABills
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.RaBillId);

            if (raBill == null)
            {
                throw new NotFoundException($"RA Bill does not exist with Id: {request.RaBillId}");
            }

            raBill.SetTitle(request.Data.Title);
            raBill.SetBillDate((DateTime)request.Data.BillDate);
            
            return Unit.Value;

        }
    }

}
