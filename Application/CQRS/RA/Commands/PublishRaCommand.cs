using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Commands;

public record PublishRaCommand(int raId) : IRequest
{
}

public class PublishRaCommandHandler : IRequestHandler<PublishRaCommand>
{
    private readonly IAppDbContext _db;

    public PublishRaCommandHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Unit> Handle(PublishRaCommand request, CancellationToken cancellationToken)
    {
        var ra = await _db.RAHeaders.Include(r => r.Items).FirstOrDefaultAsync(p => p.Id == request.raId);
        if (ra == null)
        {
            throw new NotFoundException(nameof(ra), request.raId);
        }
        ra.Status = RAStatus.Published;
        var worder = await _db.WorkOrders.Include(w => w.Items)
                          .SingleOrDefaultAsync(p => p.Id == ra.WorkOrderId);
        if (worder == null)
        {
            throw new NotFoundException(nameof(worder), ra.WorkOrderId);
        }
        foreach (var item in ra.Items)
        {
            worder.AddRAQuantity(item.CurrentRAQty, item.WorkOrderItemId);
        }
        await _db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}