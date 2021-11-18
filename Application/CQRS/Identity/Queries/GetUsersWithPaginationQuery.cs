using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Identity;
using Shared.Requests;
using Shared.Responses;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Queries
{
    public class GetUsersWithPaginationQuery : PagedRequest, IRequest<PaginatedList<UserDto>>
    {
    }

    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, PaginatedList<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private Expression<Func<AppUser, bool>> Criteria { set; get; }

        public GetUsersWithPaginationQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserDto>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                Criteria = (m =>
                    m.UserName.Contains(request.Search) ||
                    m.DisplayName.ToLower().Contains(request.Search.ToLower()) ||
                    m.PhoneNumber.Contains(request.Search.ToLower())
                );

                query = query.Where(Criteria);
            }

            return await query
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
