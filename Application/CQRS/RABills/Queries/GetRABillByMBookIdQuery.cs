using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Queries
{
    public record GetRABillByMBookIdQuery(int MBookId) : IRequest<List<RABillResponse>>
    {
    }

    public class GetRABillByMBookIdQueryHandler : IRequestHandler<GetRABillByMBookIdQuery, List<RABillResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetRABillByMBookIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RABillResponse>> Handle(GetRABillByMBookIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.RABills
                .Include(p => p.Items)
                .Where(p => p.MeasurementBookId == request.MBookId)
                .OrderBy(p => p.BillDate)
                .AsNoTracking()
                .ProjectToListAsync<RABillResponse>(_mapper.ConfigurationProvider);
        }
    }
}
