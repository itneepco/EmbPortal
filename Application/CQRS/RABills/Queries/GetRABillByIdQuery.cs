using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Queries
{
    public record GetRABillByIdQuery(int Id) : IRequest<RABillResponse>
    {
    }

    public class GetRABillByIdQueryHandler : IRequestHandler<GetRABillByIdQuery, RABillResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetRABillByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RABillResponse> Handle(GetRABillByIdQuery request, CancellationToken cancellationToken)
        {
            var raBill = await _context.RABills
                .Include(p => p.Items)
                .ProjectTo<RABillResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (raBill == null)
            {
                throw new NotFoundException(nameof(raBill), request.Id);
            }

            return raBill;
        }
    }
}
