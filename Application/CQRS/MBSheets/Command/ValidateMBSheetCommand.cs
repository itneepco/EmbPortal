using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record ValidateMBSheetCommand(int Id) : IRequest
    {
    }

    public class ValidateMBSheetCommandHandler : IRequestHandler<ValidateMBSheetCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ValidateMBSheetCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ValidateMBSheetCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets.FindAsync(request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(mbSheet), request.Id);
            }

            if (mbSheet.Status != MBSheetStatus.CREATED)
            {
                throw new BadRequestException("MB Sheet has already been validated");
            }

            if (mbSheet.ValidationOfficer != _currentUserService.EmployeeCode)
            {
                throw new BadRequestException("Only Validation Officer can validate MB Sheet");
            }

            mbSheet.MarkAsValidated();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
