using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record ApproveRABillCommand(int Id) : IRequest
    {
    }

    public class ApproveRABillCommandHandler : IRequestHandler<ApproveRABillCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ApproveRABillCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ApproveRABillCommand request, CancellationToken cancellationToken)
        {
            var raBill = await _context.RABills.FindAsync(request.Id);


            if (raBill == null)
            {
                throw new NotFoundException(nameof(raBill), request.Id);
            }

            if (raBill.Status == RABillStatus.APPROVED)
            {
                throw new BadRequestException("RA Bill has already been approved");
            }

            if (raBill.EicEmpCode != _currentUserService.EmployeeCode)
            {
                throw new BadRequestException("Only Engineer-in-charge can accept the MB Sheet");
            }

            raBill.MarkAsApproved();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
