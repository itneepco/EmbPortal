using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using MediatR;
using Shared.Request;

namespace Application.WorkOrders.Command
{
   public class CreateWorkOrderCommand : CreateWorkOrderRequest, IRequest<int>
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
            agreementNo : request.AggrementNo,
            agreementDate : request.AggrementDate,
            projectId : request.ProjectId,
            contractorId : request.ContractorId,
            engineerInCharge : "Current User" //to be implemented
        );
        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync(cancellationToken);
        return workOrder.Id;
      }
   }
}