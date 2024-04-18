using Application.Interfaces;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Dashboard.Queries
{
    public class DashboardStatsQuery : IRequest<DashboardStatsResponse>
    {
    }

    public class DashboardStatsQueryHandler : IRequestHandler<DashboardStatsQuery, DashboardStatsResponse>
    {
        private readonly IAppDbContext _context; 
        private readonly ICurrentUserService _currentUserService;

        public DashboardStatsQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<DashboardStatsResponse> Handle(DashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var empCode = _currentUserService.EmployeeCode;

            var mbApproval = await _context.MBSheets
                                     .Where(p => p.Status == MBSheetStatus.VALIDATED
                                                 && p.EicEmpCode == empCode)
                                     .CountAsync();

            var mbValidation = await _context.MBSheets
                                    .Where(p => (p.ValidatorEmpCode == empCode || p.EicEmpCode == empCode)
                                                 && p.Status == MBSheetStatus.PUBLISHED)
                                    .CountAsync();



            var workOrderCount = await _context.WorkOrders
                                     .Where(p => (p.CreatedBy == empCode || p.EngineerInCharge == empCode))
                                     .CountAsync();
            var mBookCount = await _context.MeasurementBooks
                                     .Where(p => (p.MeasurerEmpCode == empCode || p.ValidatorEmpCode == empCode || p.EicEmpCode == empCode))
                                     .CountAsync();

            return new DashboardStatsResponse
            {
                MBSheetValidation = mbValidation,
                MBSheetApproval = mbApproval,
                WorkOrderCount = workOrderCount,
                MBookCount = mBookCount
            };
        }
    }
}
