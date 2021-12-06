using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using EmbPortal.Shared.Requests;
using System.Threading;
using System.Threading.Tasks;

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

            workOrder.SetOrderNo(request.data.WorkOrderNo);
            workOrder.SetOrderDate(request.data.OrderDate);
            workOrder.SetTitle(request.data.Title);
            workOrder.SetAgreementNo(request.data.AgreementNo);
            workOrder.SetAgreementDate(request.data.AgreementDate);
            workOrder.SetProjectId(request.data.ProjectId);
            workOrder.SetContractorId(request.data.ContractorId);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
