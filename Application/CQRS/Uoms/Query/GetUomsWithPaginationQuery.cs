using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Uoms.Query
{
    public record GetUomsWithPaginationQuery(PagedRequest Data) : IRequest<PaginatedList<UomResponse>>
    {
    }

    public class GetUomsWithPaginationQueryHandler : IRequestHandler<GetUomsWithPaginationQuery, PaginatedList<UomResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private Expression<Func<Uom, bool>> Criteria { set; get; }

        public GetUomsWithPaginationQueryHandler(IMapper mapper, IAppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<UomResponse>> Handle(GetUomsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Uoms.AsQueryable();

            if (!string.IsNullOrEmpty(request.Data.Search))
            {
                Criteria = (m => 
                    m.Name.ToLower().Contains(request.Data.Search.ToLower()) ||
                    m.Description.ToLower().Contains(request.Data.Search.ToLower())
                );
                query = query.Where(Criteria);
            }

            return await query.OrderBy(p => p.Name)
                .ProjectTo<UomResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.Data.PageNumber, request.Data.PageSize);
        }
    }
}
