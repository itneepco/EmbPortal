using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Requests.MeasurementBooks;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command;

public record ChangeMeasurerCommand(int id, ChangeOfficerRequest data) : IRequest
{
}

public class ChangeMeasurerCommandHnadler : IRequestHandler<ChangeMeasurerCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public ChangeMeasurerCommandHnadler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(ChangeMeasurerCommand request, CancellationToken cancellationToken)
    {
        var mBook = await _context.MeasurementBooks.Include(m => m.WorkOrder)
                                                   .FirstOrDefaultAsync(p => p.Id == request.id);

        if (mBook == null)
        {
            throw new NotFoundException(nameof(mBook), request.id);
        }

        var currentUser = _currentUserService.EmployeeCode;

        if (!currentUser.Equals(mBook.WorkOrder.EngineerInCharge))
        {
            throw new UnauthorizedUserException("Only Engineer In Charge can change Measurement Officer");
        }

        mBook.SetMeasurementOfficer(request.data.Officer);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
