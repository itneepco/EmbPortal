using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Query
{
    public class GetPendingApprovalMBSheetQuery : IRequest<IList<MBSheetInfoResponse>>
    {
    }

    public class GetPendingApprovalMBSheetQueryHandler : IRequestHandler<GetPendingApprovalMBSheetQuery, IList<MBSheetInfoResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetPendingApprovalMBSheetQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IList<MBSheetInfoResponse>> Handle(GetPendingApprovalMBSheetQuery request, CancellationToken cancellationToken)
        {
            var empCode = _currentUserService.EmployeeCode;

            return await _context.MBSheets
              .Include(p => p.MeasurementBook)
              .Where(p => p.Status == MBSheetStatus.VALIDATED && p.AcceptingOfficer == empCode)
              .AsNoTracking()
              .ProjectToListAsync<MBSheetInfoResponse>(_mapper.ConfigurationProvider);
        }
    }
}
