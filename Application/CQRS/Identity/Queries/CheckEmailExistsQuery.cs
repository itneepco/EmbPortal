using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Queries;

public class CheckEmailExistsQuery : IRequest<bool>
{
    public string Email { get; set; }
}

public class CheckEmailExistsQueryHandler : IRequestHandler<CheckEmailExistsQuery, bool>
{
    private readonly UserManager<AppUser> _userManager;

    public CheckEmailExistsQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
    {
        return (await _userManager.FindByEmailAsync(request.Email)) != null;
    }
}
