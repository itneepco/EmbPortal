using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IMBSheetService
    {
        Task<List<MBSheetInfoResponse>> GetMBSheetsByMBookId(int mBookId);
        Task<IResult<MBSheetResponse>> GetMBSheetById(int mbSheetId);
        Task<List<MBSheetInfoResponse>> GetPendingValidationMBSheets();
        Task<List<MBSheetInfoResponse>> GetPendingApprovalMBSheets();
        Task<IResult<int>> CreateMBSheet(MBSheetRequest request);
        Task<IResult> EditMBSheet(int mbSheetId, MBSheetRequest request);
        Task<IResult> PublishMBSheet(int mbSheetId);
        Task<IResult> DeleteMBSheet(int mbSheetId);
        Task<IResult> ValidateMBSheet(int mbSheetId);
        Task<IResult> AcceptMBSheet(int mbSheetId);
        Task<IResult<int>> CreateMBSheetItem(int mbSheetId, MBSheetItemRequest request);
        Task<IResult> UpdateMBSheetItem(int mbSheetId, int itemId, MBSheetItemRequest request);
        Task<IResult> DeleteMBSheetItem(int mbSheetId, int itemId);
        Task<IResult<List<UploadResult>>> UploadFiles(int mbSheetId, int itemId, MultipartFormDataContent content);
        Task<IResult> DeleteMBSheetItemAttachment(int mbSheetId, int itemId, int attachmentId);
        Task<string> DownloadMBSheetItemAttachment(int mbSheetId, int itemId, int attachmentId);
    }
}
