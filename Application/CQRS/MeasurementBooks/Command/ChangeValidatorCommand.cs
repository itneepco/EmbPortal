using Application.Exceptions;
using Application.Interfaces;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using EmbPortal.Shared.Requests.MeasurementBooks;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.MeasurementBooks.Command;

public record ChangeValidatorCommand(int id, ChangeOfficerRequest data) : IRequest
{
}

public class ChangeValidatorCommandHnadler : IRequestHandler<ChangeValidatorCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public ChangeValidatorCommandHnadler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(ChangeValidatorCommand request, CancellationToken cancellationToken)
    {
        var mBook = await _context.MeasurementBooks.FirstOrDefaultAsync(p => p.Id == request.id);

        if (mBook == null)
        {
            throw new NotFoundException(nameof(mBook), request.id);
        }

        var currentUser = _currentUserService.EmployeeCode;

        if (!currentUser.Equals(mBook.EicEmpCode))
        {
            throw new UnauthorizedUserException("Only Engineer In Charge can change Validating Officer");
        }

        mBook.SetValidatingOfficer(request.data.Officer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}