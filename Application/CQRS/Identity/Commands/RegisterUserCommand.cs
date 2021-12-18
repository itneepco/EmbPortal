using Application.Exceptions;
using Domain.Entities.Identity;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Commands.RegisterUser
{
    public record RegisterUserCommand(UserRequest Data) : IRequest<string>
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
                DisplayName = request.Data.DisplayName,
                Email = request.Data.Email,
                UserName = request.Data.EmployeeCode,
                Designation = request.Data.Designation
            };

            // Default password will be "Pass@123"
            var result = await _userManager.CreateAsync(user, "Pass@123");

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to register user");
            }

            // Assign roles to user
            foreach (var role in request.Data.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }            

            return user.Id;
        }
    }
}

