using Domain.Entities.Identity;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Queries
{
    public class GetCurrentUserQuery : IRequest<AuthUserDto>
    {
        public string Email { get; set; }
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, AuthUserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public GetCurrentUserQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            return new AuthUserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
