using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.WorkOrders.Response;
using Domian.WorkOrderAggregate;
using MediatR;

namespace Application.WorkOrders.Command
{
   public class CreateWorkOrderCommand : CreateWorkOrderDto, IRequest<int>
   {

   }

   public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, int>
   {
      private readonly IAppDbContext _context;
      public CreateWorkOrderCommandHandler(IAppDbContext context)
      {
         _context = context;

      }

      public async Task<int> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
      {
        var workOrder = new WorkOrder
        (
            orderNo : request.WorkOrderNo,
            orderDate : request.OrderDate,
            title : request.Title,
            aggrementNo : request.AggrementNo,
            aggrementDate : request.AggrementDate,
            projectId : request.ProjectId,
            contractorId : request.ContractorId
        );
        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync(cancellationToken);
        return workOrder.Id;
      }
   }
}