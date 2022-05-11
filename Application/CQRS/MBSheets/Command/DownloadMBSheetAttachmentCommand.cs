using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record DownloadMBSheetAttachmentCommand(int MBSheetId, int ItemId, int AttachmentId, string ContentRoot) : IRequest<byte[]>
    {
    }

    public class DownloadMBSheetAttachmentCommandHandler : IRequestHandler<DownloadMBSheetAttachmentCommand, byte[]>
    {
        private readonly IAppDbContext _context;

        public DownloadMBSheetAttachmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> Handle(DownloadMBSheetAttachmentCommand request, CancellationToken cancellationToken)
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

            var path = Path.Combine(request.ContentRoot, FileConstant.FolderName, attachment.FileNormalizedName);

            if (!File.Exists(path)) // check if file exist
            {
                throw new NotFoundException($"No such attachment found on the server: {attachment.FileName}");
            }

            Byte[] bytes = File.ReadAllBytes(path);

            return bytes;
        }
    }
}
