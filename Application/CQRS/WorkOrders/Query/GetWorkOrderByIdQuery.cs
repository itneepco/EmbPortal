using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        private readonly ICurrentUserService _currentUserService;

        public GetWorkOrderByIdQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<WorkOrderResponse> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var currEmpCode = _currentUserService.EmployeeCode;
            var workOrder = await _context.WorkOrders
                .Include(p => p.Engineer)
                .Include(p => p.Items)
                   .ThenInclude(s => s.Uom)
                .Where(p => p.CreatedBy == currEmpCode || p.EngineerInCharge == currEmpCode)
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
