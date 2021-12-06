using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using Infrastructure.Interfaces;
using MediatR;
using EmbPortal.Shared.Requests;

namespace Application.WorkOrders.Command
{
    public record CreateWorkOrderCommand(WorkOrderRequest data) : IRequest<int>
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
            var workOrder = new WorkOrder
            (
                orderNo: request.data.WorkOrderNo,
                orderDate: request.data.OrderDate,
                title: request.data.Title,
                agreementNo: request.data.AgreementNo,
                agreementDate: request.data.AgreementDate,
                projectId: request.data.ProjectId,
                contractorId: request.data.ContractorId,
                engineerInCharge: "001234" //_currentUserService.EmployeeCode
            );

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return workOrder.Id;
        }
    }
}