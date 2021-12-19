using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Query
{
    public record GetOrdersByCreatorPaginationQuery(PagedRequest data) : IRequest<PaginatedList<WorkOrderResponse>>
    {
    }

    public class GetOrdersByCreatorPaginationQueryHandler : IRequestHandler<GetOrdersByCreatorPaginationQuery, PaginatedList<WorkOrderResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetOrdersByCreatorPaginationQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedList<WorkOrderResponse>> Handle(GetOrdersByCreatorPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.WorkOrders
                .Include(p => p.Project)
                .Include(p => p.Contractor)
                .Where(p => p.EngineerInCharge == _currentUserService.EmployeeCode)
                .OrderByDescending(p => p.Created)
                .ProjectTo<WorkOrderResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}
