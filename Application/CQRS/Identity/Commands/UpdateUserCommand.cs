using Application.Exceptions;
using Domain.Entities.Identity;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Commands.UpdateUser
{
    public record UpdateUserCommand(string Id, UserRequest Data) : IRequest<string>
    {
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;
        public UpdateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), request.Id);
            }

            user.DisplayName = request.Data.DisplayName;
            user.Email = request.Data.Email;
            user.UserName = request.Data.EmployeeCode;
            user.Designation = request.Data.Designation;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to update user");
            }

            // Remove old roles
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            // Update with new roles
            foreach (var role in request.Data.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return user.Id;
        }
    }
}
