using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using EmbPortal.Shared.Responses;
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
    public record GetMBSheetsByMBookIdQuery(int MBookId) : IRequest<List<MBSheetResponse>>
    {
    }

    public class GetMBSheetsByMBookIdQueryHandler : IRequestHandler<GetMBSheetsByMBookIdQuery, List<MBSheetResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetMBSheetsByMBookIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MBSheetResponse>> Handle(GetMBSheetsByMBookIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.MBSheets
                .Include(p => p.Items)
                    .ThenInclude(i => i.Attachments)
                .Where(p => p.MeasurementBookId == request.MBookId)
                .OrderBy(p => p.MeasurementDate)
                .AsNoTracking()
                .ProjectToListAsync<MBSheetResponse>(_mapper.ConfigurationProvider);
        }
    }
}
