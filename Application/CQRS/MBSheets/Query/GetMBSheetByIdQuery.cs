using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                .ProjectTo<MBSheetResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(mbSheet), request.Id);
            }

            return mbSheet;
        }
    }
}
