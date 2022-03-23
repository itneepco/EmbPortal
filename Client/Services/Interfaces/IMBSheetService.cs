using Client.Models;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IMBSheetService
    {
        Task<List<MBSheetResponse>> GetMBSheetsByMBookId(int mBookId);
        Task<IResult<MBSheetResponse>> GetMBSheetById(int mbSheetId);
        Task<IResult<int>> CreateMBSheet(MBSheetRequest request);
        Task<IResult> EditMBSheet(int mbSheetId, MBSheetRequest request);
        Task<IResult> DeleteMBSheet(int mbSheetId);
        Task<IResult> ValidateMBSheet(int mbSheetId);
        Task<IResult> AcceptMBSheet(int mbSheetId);
        Task<IResult<int>> CreateMBSheetItem(int mbSheetId, MBSheetItemRequest request);
        Task<IResult> UpdateMBSheetItem(int mbSheetId, int itemId, MBSheetItemRequest request);
        Task<IResult> DeleteMBSheetItem(int mbSheetId, int itemId);
    }
}
