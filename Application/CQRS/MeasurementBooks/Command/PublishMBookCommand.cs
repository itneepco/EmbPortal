using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command;

public record PublishMBookCommand(int Id) : IRequest
{
}

public class PublishMBookCommandHandler : IRequestHandler<PublishMBookCommand>
{
    private readonly IAppDbContext _context;

    public PublishMBookCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PublishMBookCommand request, CancellationToken cancellationToken)
    {
        var mBook = await _context.MeasurementBooks.FindAsync(request.Id);

        if (mBook == null)
        {
            throw new NotFoundException(nameof(mBook), request.Id);
        }

        if (mBook.Status == MBookStatus.COMPLETED)
        {
            throw new BadRequestException("The measurement book has already been completed");
        }

        mBook.MarkPublished();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
