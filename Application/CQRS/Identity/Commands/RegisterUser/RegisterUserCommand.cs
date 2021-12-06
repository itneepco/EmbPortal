using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using EmbPortal.Shared.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Commands.RegisterUser
{
    public class RegisterUserCommand : RegisterDto, IRequest<string>
    {
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.EmployeeCode
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to register user");
            }

            var roleAddResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleAddResult.Succeeded)
            {
                throw new BadRequestException("Failed to add role");
            }

            return user.Id;
        }
    }
}

