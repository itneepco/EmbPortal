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

            var userQuery = _context.AppUsers.AsQueryable();
            var msheetQuery = _context.MBSheets.AsQueryable();

            var query = from msheet in msheetQuery
                        join measurer in userQuery on msheet.MeasurerEmpCode equals measurer.UserName
                        join validator in userQuery on msheet.ValidatorEmpCode equals validator.UserName
                        join eic in userQuery on msheet.EicEmpCode equals eic.UserName
                        select new { msheet, measurer, validator, eic };

            var results = await query
                .Where(p => p.msheet.Status == MBSheetStatus.PUBLISHED &&
                                               (p.msheet.ValidatorEmpCode == empCode || p.msheet.EicEmpCode == empCode))
                .OrderBy(p => p.msheet.MeasurementDate)
                .AsNoTracking()
                .ToListAsync();

            List<MBSheetInfoResponse> response = new();
            foreach (var result in results)
            {
                var item = _mapper.Map<MBSheetInfoResponse>(result.msheet);
                item.MeasurerName = result.measurer.DisplayName;
                item.ValidatorName = result.validator.DisplayName;
                item.EicName = result.eic.DisplayName;
                response.Add(item);
            }

            return response;
        }
    }
}
