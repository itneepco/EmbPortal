using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command;

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

    public async Task Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var workOrder = await _context.WorkOrders.FindAsync(request.id);
        
        if (workOrder == null)
        {
            throw new NotFoundException(nameof(workOrder), request.id);
        }
       
        var mBook = await _context.MeasurementBooks.FirstOrDefaultAsync(p => p.WorkOrderId == workOrder.Id);         

        if (mBook != null)
        {
            throw new BadRequestException("Cannot delete as MeasurementBooks exists for the order");
        }
        _context.WorkOrders.Remove(workOrder);
        await _context.SaveChangesAsync(cancellationToken);
        
    }
}
