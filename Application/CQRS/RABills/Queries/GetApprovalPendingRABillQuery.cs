using Application.Interfaces;
using AutoMapper;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Queries;

public class GetApprovalPendingRABillQuery : IRequest<IList<RABillInfoResponse>>
{
}

public class GetApprovalPendingRABillQueryHandler : IRequestHandler<GetApprovalPendingRABillQuery, IList<RABillInfoResponse>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetApprovalPendingRABillQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IList<RABillInfoResponse>> Handle(GetApprovalPendingRABillQuery request, CancellationToken cancellationToken)
    {
        var wOrderQuery = _context.WorkOrders.AsQueryable();
        var mBookQuery  = _context.MeasurementBooks.AsQueryable();
        var raBillQuery = _context.RABills
                           .Include(p => p.Items)
                           .AsQueryable();

        var query = from r in raBillQuery
                    join w in wOrderQuery on r.WorkOrderId equals w.Id
                    join m in mBookQuery on r.MeasurementBookId equals m.Id
                    select new { w,m,r};


        var result = await query
                     .Where(p => p.r.EicEmpCode == _currentUserService.EmployeeCode &&
                     (p.r.Status == RABillStatus.CREATED || p.r.Status == RABillStatus.REVOKED))
                     .OrderBy(p => p.r.BillDate)
                     .AsNoTracking()
                     .Select( p => new RABillInfoResponse
                     {
                         Id = p.r.Id,
                         RABillTitle = p.r.Title,
                         MBookTitle = p.m.Title,
                         MeasurementBookId = p.m.Id,
                         OrderNo = p.w.OrderNo.ToString(),
                         OrderDate = p.w.OrderDate,
                         Contractor = p.w.Contractor

                     })
                     .ToListAsync();            
                     //.ProjectToListAsync<RABillInfoResponse>(_mapper.ConfigurationProvider);
       return result;
    }
}
