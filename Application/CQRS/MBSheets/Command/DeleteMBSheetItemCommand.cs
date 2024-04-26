using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record DeleteMBSheetItemCommand(int Id, int MBSheetId, string ContentRoot) : IRequest
    {
    }

    public class DeleteMBSheetItemCommandHandler : IRequestHandler<DeleteMBSheetItemCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteMBSheetItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteMBSheetItemCommand request, CancellationToken cancellationToken)
        {
            MBSheet mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                    .ThenInclude(i => i.Attachments)
                .Where(p => p.Id == request.MBSheetId)
                .FirstOrDefaultAsync();

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.MBSheetId);
            }

            var mbSheetItem = mbSheet.Items.FirstOrDefault(p => p.Id == request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException($"Current MB Sheet does not have line item with Id: {request.Id}");
            }

            mbSheet.RemoveLineItem(mbSheetItem);

            // Remove all attachments related with this MB Sheet item
            foreach (var attachment in mbSheetItem.Attachments)
            {
                var path = Path.Combine(request.ContentRoot, FileConstant.FolderName, attachment.FileNormalizedName);

                if (File.Exists(path)) // check if file exist
                {
                    File.Delete(path); // delete file from storage
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
