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
    public record CreateWorkOrderCommand(CreateWorkOrderRequest data) : IRequest<int>
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
            if (request.data.Items.Count == 0)
            {
                throw new BadRequestException("Work order line items cannot be empty");
            }

            var workOrder = new WorkOrder
            (
                orderNo: request.data.WorkOrderNo,
                orderDate: request.data.OrderDate,
                title: request.data.Title,
                agreementNo: request.data.AgreementNo,
                agreementDate: request.data.AgreementDate,
                projectId: request.data.ProjectId,
                contractorId: request.data.ContractorId,
                engineerInCharge: _currentUserService.EmployeeCode
            );

            foreach (var item in request.data.Items)
            {
                workOrder.AddUpdateLineItem(
                    description: item.Description,
                    itemNo: item.ItemNo,
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