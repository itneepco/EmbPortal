using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record DeleteWorkOrderCommand(int id) : IRequest
    {
    }

    public class DeleteWorkOrderCommandHandler : IRequestHandler<DeleteWorkOrderCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteWorkOrderCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders.FindAsync(request.id);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.id);
            }

            if(workOrder.Status == WorkOrderStatus.PUBLISHED)
            {
                throw new BadRequestException("Published work order cannot be deleted");
            }

            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
