using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identity.Queries
{
    public record GetUserRolesQuery(string userId) : IRequest<IList<string>>
    {
    }

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IList<string>>
    {
        private readonly UserManager<AppUser> userManager;

        public GetUserRolesQueryHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IList<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);

            if(user == null)
            {
                throw new NotFoundException(nameof(user), request.userId);
            }

            var roles = await userManager.GetRolesAsync(user);

            return roles;
        }
    }
}
