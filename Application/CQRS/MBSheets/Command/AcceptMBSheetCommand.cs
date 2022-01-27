using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record AcceptMBSheetCommand(int Id) : IRequest
    {
    }

    public class AcceptMBSheetCommandHandler : IRequestHandler<AcceptMBSheetCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AcceptMBSheetCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(AcceptMBSheetCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets.FindAsync(request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(mbSheet), request.Id);
            }

            if (mbSheet.Status != MBSheetStatus.VALIDATED)
            {
                throw new BadRequestException("Please validate the MB Sheet first");
            }

            if (mbSheet.AcceptingOfficer != _currentUserService.EmployeeCode)
            {
                throw new BadRequestException("Only Engineer-in-charge can accept the MB Sheet");
            }

            mbSheet.MarkAsAccepted();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
