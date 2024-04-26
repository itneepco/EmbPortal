using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identity.Commands
{
    public record ResetUserPasswordCommand(string UserId) : IRequest
    {
    }

    public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand>
    {
        private readonly UserManager<AppUser> userManager;
        public ResetUserPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), request.UserId);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, "Pass@123");
        }
    }
}
