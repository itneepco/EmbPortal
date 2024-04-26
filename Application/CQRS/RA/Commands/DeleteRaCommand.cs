using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RA.Commands;

public record DeleteRa(int Id) : IRequest
{
}

public class Handler: IRequestHandler<DeleteRa>
{
    private readonly IAppDbContext _db;
    public Handler(IAppDbContext db)
    {
        _db = db;
    }

    public  async Task Handle(DeleteRa request, CancellationToken cancellationToken)
    {
        var ra = await _db.RAHeaders
                .Include(p => p.Items)
                .Include(p =>p.Deductions)
                .SingleOrDefaultAsync(i => i.Id == request.Id);
        if (ra == null)
        {
            throw new NotFoundException(nameof(ra), request.Id);
        }


        _db.RAHeaders.Remove(ra);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
