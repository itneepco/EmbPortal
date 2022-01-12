using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record PublishWorkOrderCommand(int Id) : IRequest
    {
    }

    public class PublishWorkOrderCommandHandler : IRequestHandler<PublishWorkOrderCommand>
    {
        private readonly IAppDbContext _context;

        public PublishWorkOrderCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PublishWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders.FindAsync(request.Id);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.Id);
            }

            if (workOrder.Status == WorkOrderStatus.COMPLETED)
            {
                throw new BadRequestException("The work order has already been completed");
            }

            workOrder.MarkPublished();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
