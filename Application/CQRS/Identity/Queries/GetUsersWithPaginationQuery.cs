using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity.Queries
{
    public record GetUsersWithPaginationQuery(PagedRequest Data) : IRequest<PaginatedList<UserResponse>>
    {
    }

    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, PaginatedList<UserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private Expression<Func<AppUser, bool>> Criteria { set; get; }

        public GetUsersWithPaginationQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserResponse>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Data.Search))
            {
                Criteria = (m =>
                    m.UserName.Contains(request.Data.Search) ||
                    m.DisplayName.ToLower().Contains(request.Data.Search.ToLower()) ||
                    m.PhoneNumber.Contains(request.Data.Search.ToLower())
                );

                query = query.Where(Criteria);
            }

            return await query
                .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.Data.PageNumber, request.Data.PageSize);
        }
    }
}
