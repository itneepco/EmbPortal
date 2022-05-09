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
    public record RemoveMBSheetAttachmentCommand(int MBSheetId, int ItemId, int AttachmentId, string ContentRoot) : IRequest
    {
    }

    public class RemoveMBSheetAttachmentCommandHandler : IRequestHandler<RemoveMBSheetAttachmentCommand>
    {
        private readonly IAppDbContext _context;

        public RemoveMBSheetAttachmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveMBSheetAttachmentCommand request, CancellationToken cancellationToken)
        {
            var mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                    .ThenInclude(i => i.Attachments)    
                .Where(p => p.Id == request.MBSheetId)
                .FirstOrDefaultAsync();

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.MBSheetId);
            }

            var mbSheetItem = mbSheet.Items.FirstOrDefault(p => p.Id == request.ItemId);

            if (mbSheet == null)
            {
                throw new NotFoundException($"Current MB Sheet does not have line item with Id: {request.ItemId}");
            }

            var attachment = mbSheetItem.Attachments.FirstOrDefault(p => p.Id == request.AttachmentId);

            if (attachment == null)
            {
                throw new NotFoundException($"MB Sheet Item does not have attachment with Id: {request.AttachmentId}");
            }

            mbSheetItem.RemoveAttachment(attachment);

            var path = Path.Combine(request.ContentRoot, FileConstant.FolderName, attachment.FileNormalizedName);

            if(File.Exists(path)) // check if file exist
            {
                File.Delete(path); // delete file from storage
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
