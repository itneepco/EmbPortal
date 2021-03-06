using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Queries
{
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
            return await _context.RABills
               .Include(p => p.Items)
               .Include(p => p.MeasurementBook)
                    .ThenInclude(m => m.WorkOrder)
                        .ThenInclude(o => o.Contractor)
               .Where(p => p.AcceptingOfficer == _currentUserService.EmployeeCode && p.Status != EmbPortal.Shared.Enums.RABillStatus.APPROVED)
               .OrderBy(p => p.BillDate)
               .AsNoTracking()
               .ProjectToListAsync<RABillInfoResponse>(_mapper.ConfigurationProvider);
        }
    }
}
