using EmbPortal.Shared.Constants;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record UploadMBSheetAttachmentsCommand(IEnumerable<IFormFile> Files, string ContentRoot) : IRequest<List<UploadResult>>
    {
    }

    public class UploadMBSheetAttachmentsCommandHandler : IRequestHandler<UploadMBSheetAttachmentsCommand, List<UploadResult>>
    {
        private readonly ILogger<UploadMBSheetAttachmentsCommand> logger;

        public UploadMBSheetAttachmentsCommandHandler(ILogger<UploadMBSheetAttachmentsCommand> logger)
        {
            this.logger = logger;
        }

        public async Task<List<UploadResult>> Handle(UploadMBSheetAttachmentsCommand request, CancellationToken cancellationToken)
        {
            var maxAllowedFiles = FileConstant.MaxFilesCount;
            long maxFileSize = FileConstant.MaxFileSize;
            var filesProcessed = 0;
            List<UploadResult> uploadResults = new();

            foreach (var file in request.Files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        logger.LogInformation("{FileName} length is 0 (Err: 1)",
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes (Err: 2)",
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            trustedFileNameForFileStorage = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                            var path = Path.Combine(request.ContentRoot, "Files", trustedFileNameForFileStorage);

                            await using FileStream fs = new(path, FileMode.Create);
                            await file.CopyToAsync(fs);

                            logger.LogInformation("{FileName} saved at {Path}",
                                trustedFileNameForDisplay, path);
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = trustedFileNameForFileStorage;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        trustedFileNameForDisplay, maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }

                uploadResults.Add(uploadResult);
            }

            return uploadResults;
        }
    }
}
