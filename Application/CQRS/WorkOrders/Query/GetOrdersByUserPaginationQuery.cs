using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Query
{
    public record GetOrdersByUserPaginationQuery(PagedRequest data) : IRequest<PaginatedList<WorkOrderResponse>>
    {
    }

    public class GetOrdersByCreatorPaginationQueryHandler : IRequestHandler<GetOrdersByUserPaginationQuery, PaginatedList<WorkOrderResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private Expression<Func<WorkOrder, bool>> Criteria { set; get; }

        public GetOrdersByCreatorPaginationQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedList<WorkOrderResponse>> Handle(GetOrdersByUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.WorkOrders.AsQueryable();

            if (!string.IsNullOrEmpty(request.data.Search))
            {
                Criteria = (m =>
                    m.OrderNo.ToLower().Contains(request.data.Search.ToLower()) ||
                    m.Contractor.ToLower().Contains(request.data.Search.ToLower())
                );

                query = query.Where(Criteria);
            }

            if(request.data.Status > 0) // Query based on the status of the work order
            {
                Criteria = m => m.Status == (WorkOrderStatus) request.data.Status;

                query = query.Where(Criteria);
            }

            var currEmpCode = _currentUserService.EmployeeCode;
            return await query
                .Where(p => p.CreatedBy == currEmpCode || p.EngineerInCharge == currEmpCode)
                .OrderByDescending(p => p.Created)
                .ProjectTo<WorkOrderResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}
