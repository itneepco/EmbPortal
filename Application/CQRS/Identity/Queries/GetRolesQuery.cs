using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identity.Queries
{
    public record GetRolesQuery : IRequest<IReadOnlyList<string>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyList<string>>
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public GetRolesQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IReadOnlyList<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await Task.Run(() =>
            {
                return roleManager.Roles.Select(p => p.Name).ToList();
            });

            return roles.AsReadOnly();
        }
    }
}
