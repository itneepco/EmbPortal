using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands;

public record RevokeRABillCommand(int Id) : IRequest
{
}

public class RevokeRABillCommandHandler : IRequestHandler<RevokeRABillCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RevokeRABillCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(RevokeRABillCommand request, CancellationToken cancellationToken)
    {
        var raBill = await _context.RABills.FindAsync(request.Id);

        if (raBill == null)
        {
            throw new NotFoundException(nameof(raBill), request.Id);
        }

        if (raBill.Status != RABillStatus.APPROVED)
        {
            throw new BadRequestException("RA Bill has not been approved yet");
        }

        int latestRABillId = await _context.RABills.Where(p => p.MeasurementBookId == raBill.MeasurementBookId)
            .Select(p => p.Id).MaxAsync();

        if (raBill.Id != latestRABillId)
        {
            throw new BadRequestException("Only the latest RA Bill can be revoked");
        }

        raBill.MarkAsRevoked();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
