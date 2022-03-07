using Client.Models;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IRABillService
    {
        Task<List<RABillResponse>> GetRABillsByMBookId(int mBookId);
        Task<IResult<RABillResponse>> GetRABillById(int mbSheetId);
        Task<IResult<int>> CreateRABill(RABillRequest request);
        Task<IResult> EditRABill(int id, RABillRequest request);
        Task<IResult> ApproveRABill(int raBillId);
        Task<IResult> RevokeRABill(int raBillId);
        Task<IResult> DeleteRABill(int id);
    }
}
