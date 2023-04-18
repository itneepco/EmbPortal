using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Query
{
    public record GetMBSheetByIdQuery(int Id) : IRequest<MBSheetResponse>
    {
    }

    public class GetMBSheetByIdQueryHandler : IRequestHandler<GetMBSheetByIdQuery, MBSheetResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetMBSheetByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MBSheetResponse> Handle(GetMBSheetByIdQuery request, CancellationToken cancellationToken)
        {
            var userQuery = _context.AppUsers.AsQueryable();
            var msheetQuery = _context.MBSheets
                                 .Include(p => p.Items)
                                 .AsQueryable();

            var query = from msheet in msheetQuery
                        join measurer in userQuery on msheet.MeasurerEmpCode equals measurer.UserName
                        join validator in userQuery on msheet.ValidatorEmpCode equals validator.UserName
                        join eic in userQuery on msheet.EicEmpCode equals eic.UserName
                        select new { msheet, measurer, validator, eic };

            var result = await query
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.msheet.Id == request.Id);

            if (result == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.Id);
            }

            var mbSheet = _mapper.Map<MBSheetResponse>(result.msheet);

            mbSheet.MeasurerName = result.measurer.DisplayName;
            mbSheet.ValidatorName = result.validator.DisplayName;
            mbSheet.EicName = result.eic.DisplayName;

            return mbSheet;
        }
    }
}
