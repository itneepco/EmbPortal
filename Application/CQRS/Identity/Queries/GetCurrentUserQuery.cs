using Domain.Entities.Identity;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Queries
{
    public record GetCurrentUserQuery(string Email) : IRequest<AuthUserResponse>
    {
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, AuthUserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public GetCurrentUserQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            return new AuthUserResponse
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
