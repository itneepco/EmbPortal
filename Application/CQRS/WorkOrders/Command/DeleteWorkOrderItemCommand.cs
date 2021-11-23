using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record DeleteWorkOrderItemCommand(int id, int workOrderId) : IRequest
    {
    }

    public record DeleteWorkOrderItemCommandHandler : IRequestHandler<DeleteWorkOrderItemCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteWorkOrderItemCommandHandler(IAppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Unit> Handle(DeleteWorkOrderItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.workOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.workOrderId);
            }

            workOrder.RemoveLineItem(request.id);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
