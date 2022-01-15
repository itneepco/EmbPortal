using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.MeasurementBookAggregate;
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

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetMBooksByUserIdPaginationQuery(PagedRequest Data) : IRequest<PaginatedList<MBookInfoResponse>>
    {
    }

    public class GetMBooksByUserIdPaginationQueryHandler : IRequestHandler<GetMBooksByUserIdPaginationQuery, PaginatedList<MBookInfoResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;

        private Expression<Func<MeasurementBook, bool>> Criteria { set; get; }

        public GetMBooksByUserIdPaginationQueryHandler(IMapper mapper, IAppDbContext context, ICurrentUserService userService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
        }

        public async Task<PaginatedList<MBookInfoResponse>> Handle(GetMBooksByUserIdPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MeasurementBooks
                .Include(p => p.WorkOrder)
                    .ThenInclude(o => o.Contractor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Data.Search))
            {
                Criteria = (m =>
                    m.Title.ToLower().Contains(request.Data.Search.ToLower()) ||
                    m.WorkOrder.OrderNo.ToLower().Contains(request.Data.Search.ToLower()) ||
                    m.WorkOrder.Contractor.Name.ToLower().Contains(request.Data.Search.ToLower())
                );
                query = query.Where(Criteria);
            }

            Criteria = (m =>
                m.MeasurementOfficer == _userService.EmployeeCode ||
                m.ValidatingOfficer == _userService.EmployeeCode ||
                m.WorkOrder.EngineerInCharge == _userService.EmployeeCode
            );
            query = query.Where(Criteria);


            return await query.OrderBy(p => p.WorkOrder.OrderDate)
                .ProjectTo<MBookInfoResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.Data.PageNumber, request.Data.PageSize);
        }
    }
}
