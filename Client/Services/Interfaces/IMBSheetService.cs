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
        Task<IResult<MBSheetResponse>> GetMBSheetsById(int mbSheetId);
        Task<IResult<int>> CreateMBSheet(MBSheetRequest request);
        Task<IResult> DeleteMBSheet(int id);
        Task<IResult> ValidateMBSheet(int id);
        Task<IResult> AcceptMBSheet(int id);
    }
}
