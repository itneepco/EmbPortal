using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.WorkOrders.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkOrders.Query
{
   public record GetWorkOrdersByProjectQuery(int projectId) : IRequest<IReadOnlyList<WorkOrderHeaderDto>>
   {

   }

   public class GetWorkOrdersByProjectQueryHandler : IRequestHandler<GetWorkOrdersByProjectQuery, IReadOnlyList<WorkOrderHeaderDto>>
   {
      private readonly IAppDbContext _context;
      private readonly IMapper _mapper;
      public GetWorkOrdersByProjectQueryHandler(IAppDbContext context, IMapper mapper)
      {
         _mapper = mapper;
         _context = context;
      }

      public async Task<IReadOnlyList<WorkOrderHeaderDto>> Handle(GetWorkOrdersByProjectQuery request, CancellationToken cancellationToken)
      {
         return await _context.WorkOrders
                .Where(p => p.ProjectId == request.projectId)
                .Include(p => p.Project)
                .Include(c => c.Contractor)
                .ProjectTo<WorkOrderHeaderDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
      }
   }
}