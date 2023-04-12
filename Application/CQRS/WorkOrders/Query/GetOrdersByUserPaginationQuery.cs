using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.WorkOrderAggregate;
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
            var currEmpCode = _currentUserService.EmployeeCode;

            var woQuery = _context.WorkOrders.AsQueryable();
            var userQuery = _context.AppUsers.AsQueryable();

            if (!string.IsNullOrEmpty(request.data.Search))
            {
                Criteria = (m =>
                    m.OrderNo.ToString().Contains(request.data.Search.ToLower()) ||
                    m.Contractor.ToLower().Contains(request.data.Search.ToLower())
                );

                woQuery = woQuery.Where(Criteria);
            }

            var query =  from order in woQuery 
                         join user in userQuery 
                         on order.EngineerInCharge equals user.UserName
                         where order.EngineerInCharge == currEmpCode
                         orderby order.Created
                         select new WorkOrderResponse
                         {
                             Id = order.Id,
                             OrderNo = order.OrderNo,
                             Contractor = order.Contractor,
                             OrderDate = order.Created,
                             Project = order.Project,
                             EicEmployeeCode = order.EngineerInCharge,
                             EicFullName = user.DisplayName
                         };
            
            return await query
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}
