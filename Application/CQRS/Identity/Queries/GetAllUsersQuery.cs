using Application.Mappings;
using AutoMapper;
using Domain.Entities.Identity;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identity.Queries
{
    public class GetAllUsersQuery : IRequest<IReadOnlyList<UserResponse>>
    {
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.Users
                .OrderBy(p => p.UserName)
                .AsNoTracking()
                .ProjectToListAsync<UserResponse>(_mapper.ConfigurationProvider);
        }
    }
}
