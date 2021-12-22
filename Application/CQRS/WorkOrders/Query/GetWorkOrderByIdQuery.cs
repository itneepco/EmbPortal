using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Query
{
    public record GetWorkOrderByIdQuery(int id) : IRequest<WorkOrderResponse>
    {
    }

    public class GetWorkOrderByIdQueryHandler : IRequestHandler<GetWorkOrderByIdQuery, WorkOrderResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public GetWorkOrderByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WorkOrderResponse> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Project)
                .Include(p => p.Contractor)
                .Include(p => p.Items)
                   .ThenInclude(s => s.Uom)
                .ProjectTo<WorkOrderDetailResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.id);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.id);
            }

            return workOrder;
        }
    }
}
