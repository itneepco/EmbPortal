using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Requests;

namespace Application.WorkOrders.Command
{
    public class CreateWorkOrderCommand : CreateWorkOrderRequest, IRequest<int>
    {

    }

    public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateWorkOrderCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            if(request.Items.Count == 0)
            {
                throw new BadRequestException("Work order line items cannot be zero");
            }

            var workOrder = new WorkOrder
            (
                orderNo: request.WorkOrderNo,
                orderDate: request.OrderDate,
                title: request.Title,
                agreementNo: request.AggrementNo,
                agreementDate: request.AggrementDate,
                projectId: request.ProjectId,
                contractorId: request.ContractorId,
                engineerInCharge: _currentUserService.EmployeeCode
            );

            foreach (var item in request.Items)
            {
                workOrder.AddLineItem(
                    desc: item.Description, 
                    no: item.ItemNo,
                    uomId: item.UomId, 
                    rate: item.UnitRate, 
                    poQuantity: item.PoQuantity
                );
            }

            _context.WorkOrders.Add(workOrder);

            await _context.SaveChangesAsync(cancellationToken);
            return workOrder.Id;
        }
    }
}