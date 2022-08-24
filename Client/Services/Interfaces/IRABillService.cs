using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IRABillService
    {
        Task<List<RABillResponse>> GetRABillsByMBookId(int mBookId);
        Task<List<RABillInfoResponse>> GetUserPendingRABills();
        Task<IResult<RABillDetailResponse>> GetRABillById(int mbSheetId);
        Task<IResult<int>> CreateRABill(RABillRequest request);
        Task<IResult> EditRABill(int id, RABillRequest request);
        Task<IResult> ApproveRABill(int raBillId);
        Task<IResult> RevokeRABill(int raBillId);
        Task<IResult> PostRABillToSAP(int raBillId);
        Task<IResult> DeleteRABill(int id);
        Task<string> GeneratePdf(int id);
        Task<IResult<int>> CreateRADeduction(int raBillId, RADeductionRequest request);
        Task<IResult> DeleteRADeduction(int raBillId, int deductionId);
    }
}
