using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System;
using Domain.Entities.WorkOrderAggregate;
using System.Linq.Expressions;

namespace Application.WorkOrders.Query
{
    public record GetOrdersByProjectPaginationQuery(int projectId, PagedRequest data) : IRequest<PaginatedList<WorkOrderResponse>>
    {
    }

    public class GetOrdersByProjectPaginationQueryHandler : IRequestHandler<GetOrdersByProjectPaginationQuery, PaginatedList<WorkOrderResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private Expression<Func<WorkOrder, bool>> Criteria { set; get; }

        public GetOrdersByProjectPaginationQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<WorkOrderResponse>> Handle(GetOrdersByProjectPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.WorkOrders
                .Include(p => p.Project)
                .Include(p => p.Contractor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.SubItems)
                        .ThenInclude(s => s.Uom)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.data.Search))
            {
                Criteria = (m =>
                    m.OrderNo.ToLower().Contains(request.data.Search.ToLower()) ||
                    m.Title.ToLower().Contains(request.data.Search.ToLower()) ||
                    m.Contractor.Name.ToLower().Contains(request.data.Search.ToLower())
                );

                query = query.Where(Criteria);
            }

            return await query
                .Where(p => p.ProjectId == request.projectId)
                .ProjectTo<WorkOrderResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}