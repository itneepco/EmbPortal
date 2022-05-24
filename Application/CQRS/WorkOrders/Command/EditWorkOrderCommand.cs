using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using EmbPortal.Shared.Requests;
using System.Threading;
using System.Threading.Tasks;
using System;
using EmbPortal.Shared.Enums;

namespace Application.CQRS.WorkOrders.Command
{
    public record EditWorkOrderCommand(int id, WorkOrderRequest data) : IRequest
    {
    }

    public class UpdateWorkOrderCommandHandler : IRequestHandler<EditWorkOrderCommand>
    {
        private readonly IAppDbContext _context;

        public UpdateWorkOrderCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders.FindAsync(request.id);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.id);
            }

            if (workOrder.Status != WorkOrderStatus.CREATED)
            {
                throw new BadRequestException("Published/Completed Work Order cannot be updated");
            }

            workOrder.SetOrderNo(request.data.OrderNo);
            workOrder.SetOrderDate((DateTime)request.data.OrderDate);
            workOrder.SetTitle(request.data.Title);
            workOrder.SetAgreementNo(request.data.AgreementNo);
            workOrder.SetAgreementDate((DateTime)request.data.AgreementDate);
            workOrder.SetProjectId(request.data.ProjectId);
            workOrder.SetContractorId(request.data.ContractorId);
            workOrder.SetEngineerInCharge(request.data.EngineerInCharge);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
