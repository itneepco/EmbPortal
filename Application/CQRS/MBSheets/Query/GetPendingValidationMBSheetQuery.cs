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

namespace Application.CQRS.MBSheets.Query
{
    public class GetPendingValidationMBSheetQuery : IRequest<IList<MBSheetInfoResponse>>
    {
    }
    public class GetPendingValidationMBSheetQueryHandler : IRequestHandler<GetPendingValidationMBSheetQuery, IList<MBSheetInfoResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetPendingValidationMBSheetQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IList<MBSheetInfoResponse>> Handle(GetPendingValidationMBSheetQuery request, CancellationToken cancellationToken)
        {
            var empCode = _currentUserService.EmployeeCode;

            var mBookQuery = _context.MBSheets.AsQueryable();
            var mSheetQuery = _context.MBSheets.AsQueryable();

            var query = from mbook in mBookQuery
                        join msheet in mSheetQuery
                        on mbook.Id equals msheet.MeasurementBookId
                        select new { mbook, msheet };

            var results = await query
                            .Where(p => p.msheet.Status == MBSheetStatus.PUBLISHED &&
                                        (p.msheet.ValidatorEmpCode == empCode || p.msheet.EicEmpCode == empCode))
                            .ToListAsync();

            List<MBSheetInfoResponse> response = new();
            foreach (var result in results)
            {
                var item = _mapper.Map<MBSheetInfoResponse>(result.msheet);
                item.MBookTitle = result.mbook.Title;

                response.Add(item);
            }

            return response;
        }
    }
}
