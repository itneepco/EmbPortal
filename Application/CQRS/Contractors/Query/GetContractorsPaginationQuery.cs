using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Contractors.Query
{
    public record GetContractorsPaginationQuery(PagedRequest data) : IRequest<PaginatedList<ContractorResponse>>
    {
    }

    public class GetUsersQueryWithPaginationHandler : IRequestHandler<GetContractorsPaginationQuery, PaginatedList<ContractorResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        private Expression<Func<Contractor, bool>> Criteria { set; get; }

        public GetUsersQueryWithPaginationHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ContractorResponse>> Handle(GetContractorsPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Contractors.AsQueryable();

            if (!string.IsNullOrEmpty(request.data.Search))
            {
                Criteria = (m =>
                    m.Name.ToLower().Contains(request.data.Search.ToLower())
                );

                query = query.Where(Criteria);
            }

            return await query.OrderBy(p => p.Name)
                .OrderBy(p => p.Name)
                .ProjectTo<ContractorResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}
