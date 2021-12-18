using Application.Exceptions;
using Domain.Entities.Identity;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;

namespace Application.Identity.Commands
{
    public record LoginUserCommand(LoginRequest Data) : IRequest<AuthUserResponse>
    {
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthUserResponse>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<AuthUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Data.Email);

            if (user == null)
            {
                throw new UnauthorizedUserException("Authorization unsuccessfull");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Data.Password, false);

            if (!result.Succeeded) 
            {
                throw new UnauthorizedUserException("Authorization unsuccessfull");
            };

            return new AuthUserResponse
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                EmployeeCode = user.UserName,
                Designation = user.Designation,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
