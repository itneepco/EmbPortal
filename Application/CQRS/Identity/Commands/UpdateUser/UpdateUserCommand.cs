using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Commands.UpdateUser
{
    public class UpdateUserCommand : UpdateUserDto, IRequest<string>
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
            
            user.DisplayName = request.DisplayName;
            user.Email = request.Email;
            user.UserName = request.EmployeeCode;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to update user");
            }

            return user.Id;
        }
    }
}
