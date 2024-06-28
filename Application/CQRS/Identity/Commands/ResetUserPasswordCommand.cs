using Application.Exceptions;
using Domain.Entities.Identity;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identity.Commands;

public record ResetUserPasswordCommand(string UserName, ChangePasswordRequest Model = null) : IRequest;

public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand>
{
    private readonly UserManager<AppUser> userManager;
    public ResetUserPasswordCommandHandler(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            throw new NotFoundException(nameof(user), request.UserName);
        }

        if (request.Model == null) // Password reset by admin
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, "Pass@123");

            return;
        }


        // Password reset by user
        var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, request.Model.CurrentPassword);
        if (!isCurrentPasswordValid)
        {
            throw new BadRequestException("Invalid Current Password!!");
        }

        var result = await userManager.ChangePasswordAsync(user, request.Model.CurrentPassword, request.Model.Password);
        if (!result.Succeeded)
        {
            var badRequest = new BadRequestException("Couldn't update password");
            foreach (var error in result.Errors)
            {
                badRequest.Errors.Append(error.Description);
            }
            throw badRequest;
        }
    }
}
