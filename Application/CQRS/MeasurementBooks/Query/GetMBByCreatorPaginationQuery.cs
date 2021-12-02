using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using Shared.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetMBByCreatorPaginationQuery(PagedRequest data) : IRequest<PaginatedList<WorkOrderResponse>>
    {
    }

    public class GetMBByCreatorPaginationQueryHandler : IRequestHandler<GetMBByCreatorPaginationQuery, PaginatedList<WorkOrderResponse>>
    {
        //private readonly IAppDbContext _context;
        //private readonly IMapper _mapper;
        //private readonly ICurrentUserService _currentUserService;

        //public GetMBByCreatorPaginationQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        //{
        //    _mapper = mapper;
        //    _context = context;
        //    _currentUserService = currentUserService;
        //}

        //public async Task<PaginatedList<WorkOrderResponse>> Handle(GetMBByCreatorPaginationQuery request, CancellationToken cancellationToken)
        //{
        //    return await _context.WorkOrders
        //        .Include(p => p.Project)
        //        .Include(p => p.Contractor)
        //        .Include(p => p.Items)
        //            .ThenInclude(i => i.Uom)
        //        .Where(p => p.EngineerInCharge == _currentUserService.EmployeeCode)
        //        .ProjectTo<WorkOrderResponse>(_mapper.ConfigurationProvider)
        //        .AsNoTracking()
        //        .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        //}
        public Task<PaginatedList<WorkOrderResponse>> Handle(GetMBByCreatorPaginationQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
