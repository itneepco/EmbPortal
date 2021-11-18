using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using Shared.Responses;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Contractors.Query
{
    public class GetContractorsWithPaginationQuery : PagedRequest, IRequest<PaginatedList<ContractorResponse>>
    {
    }

    public class GetUsersQueryWithPaginationHandler : IRequestHandler<GetContractorsWithPaginationQuery, PaginatedList<ContractorResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        private Expression<Func<Contractor, bool>> Criteria { set; get; }

        public GetUsersQueryWithPaginationHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ContractorResponse>> Handle(GetContractorsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Contractors.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                Criteria = (m =>
                    m.Name.ToLower().Contains(request.Search.ToLower())
                );

                query = query.Where(Criteria);
            }

            return await query.OrderBy(p => p.Name)
                .ProjectTo<ContractorResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
