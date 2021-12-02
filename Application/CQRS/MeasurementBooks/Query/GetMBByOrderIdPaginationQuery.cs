using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Requests;
using Shared.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetMBByOrderIdPaginationQuery(int workOrderId, PagedRequest data) : IRequest<PaginatedList<MeasurementBookResponse>>
    {
    }

    public class GetMBookByOrderIdPaginationQueryHandler : IRequestHandler<GetMBByOrderIdPaginationQuery, PaginatedList<MeasurementBookResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetMBookByOrderIdPaginationQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<MeasurementBookResponse>> Handle(GetMBByOrderIdPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                        .ThenInclude(w => w.Uom)
                .Where(p => p.WorkOrderId == request.workOrderId)
                .ProjectTo<MeasurementBookResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .PaginatedListAsync(request.data.PageNumber, request.data.PageSize);
        }
    }
}
