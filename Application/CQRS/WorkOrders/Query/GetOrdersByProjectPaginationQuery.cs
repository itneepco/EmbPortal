using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using Shared.Responses;

namespace Application.WorkOrders.Query
{
    public record GetOrdersByProjectPaginationQuery(int projectId, PagedRequest data) : IRequest<PaginatedList<WorkOrderResponse>>
    {
    }

    public class GetOrdersByProjectPaginationQueryHandler : IRequestHandler<GetOrdersByProjectPaginationQuery, PaginatedList<WorkOrderResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public GetOrdersByProjectPaginationQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<WorkOrderResponse>> Handle(GetOrdersByProjectPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.WorkOrders
                .Include(p => p.Project)
                .Include(p => p.Contractor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Uom)
                .Where(p => p.ProjectId == request.projectId)
                .ProjectTo<WorkOrderResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}