using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Constants;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record DeleteMBSheetCommand(int Id, string ContentRoot) : IRequest
    {
    }

    public class DeleteMBSheetCommandHandler : IRequestHandler<DeleteMBSheetCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteMBSheetCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                    .ThenInclude(i => i.Attachments)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(mbSheet), request.Id);
            }

            if (mbSheet.Status != MBSheetStatus.CREATED)
            {
                throw new BadRequestException("Validated or Accepted MB Sheet cannot be deleted");
            }

            _context.MBSheets.Remove(mbSheet);


            // Remove all attachment files when MB Sheet is removed
            foreach (var item in mbSheet.Items)
            {
                foreach (var attachment in item.Attachments)
                {
                    var path = Path.Combine(request.ContentRoot, FileConstant.FolderName, attachment.FileNormalizedName);

                    if (File.Exists(path)) // check if file exist
                    {
                        File.Delete(path); // delete file from storage
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
