using Application.Interfaces;
using Application.Mappings;
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

            return await _context.MBSheets
              .Include(p => p.MeasurementBook)
              .Where(p => p.Status == MBSheetStatus.PUBLISHED && 
                          (p.ValidationOfficer == empCode || p.AcceptingOfficer == empCode))
              .AsNoTracking()
              .ProjectToListAsync<MBSheetInfoResponse>(_mapper.ConfigurationProvider);
        }
    }
}
