using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Requests.MeasurementBooks;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command;
public record ChangeEngineerInChargeCommand(int id, ChangeOfficerRequest data) : IRequest
{
}

public class ChangeEngineerInChargeCommandHandler : IRequestHandler<ChangeEngineerInChargeCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public ChangeEngineerInChargeCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
	{
        _context = context;
        _currentUserService = currentUserService;
    }

	public async Task Handle(ChangeEngineerInChargeCommand request, CancellationToken cancellationToken)
	{
		
        var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(p => p.Id == request.id);

        if (workOrder == null)
        {
            throw new NotFoundException(nameof(workOrder), request.id);
        }

        var currentUser = _currentUserService.EmployeeCode;

        if (!currentUser.Equals(workOrder.EngineerInCharge))
        {
            throw new UnauthorizedUserException("Only Engineer In Charge is allowed for this transaction");
        }

        workOrder.SetEngineerInCharge(request.data.Officer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
