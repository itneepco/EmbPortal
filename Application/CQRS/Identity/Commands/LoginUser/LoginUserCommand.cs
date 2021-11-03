using Application.Exceptions;
using Domain.Entities.Identity;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Identity;
using System.Threading;
using System.Threading.Tasks;


namespace Application.Identity.Commands
{
    public class LoginUserCommand : LoginDto, IRequest<AuthUserDto>
    {
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthUserDto>
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

        public async Task<AuthUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedUserException("Authorization unsuccessfull");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded) 
            {
                throw new UnauthorizedUserException("Authorization unsuccessfull");
            };

            return new AuthUserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                EmployeeCode = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
