using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests.RA;
using EmbPortal.Shared.Responses;
using EmbPortal.Shared.Responses.RA;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces;

public interface IRAService
{
    Task<IResult<int>> CreateRABill(RARequest request);
    Task<IResult<RADetailResponse>> GetRAById(int raId);
    Task<List<RAResponse>> GetRAsByWOrder(int wOrderId);
    Task<List<RAResponse>> GetUserPendingRAs();
    Task<IResult> DeleteRa(int raId);
    Task<IResult> EditRa(RARequest request,int raId);
    Task<IResult> PublishRa( int raId);
    Task<string> GenrateRaReport(int raId);
    Task<IResult> PostRaToSAP(int raBillId);

}
