using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record PublishMBSheetCommand(int Id) : IRequest
    {
    }

    public class PublishMBSheetCommandHandler : IRequestHandler<PublishMBSheetCommand>
    {
        private readonly IAppDbContext _context;
        public PublishMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(PublishMBSheetCommand request, CancellationToken cancellationToken)
        {
            var mBSheet = await _context.MBSheets.FindAsync(request.Id);
            if (mBSheet == null)
            {
                throw new NotFoundException(nameof(mBSheet), request.Id);
            }

            if (mBSheet.Status == MBSheetStatus.PUBLISHED || mBSheet.Status == MBSheetStatus.VALIDATED || mBSheet.Status == MBSheetStatus.ACCEPTED)
            {
                throw new BadRequestException("MB Sheet already published");
            }

            mBSheet.MarkPublished();
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}
