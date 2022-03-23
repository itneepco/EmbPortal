using Client.Models;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IMBookService
    {
        Task<List<MBookResponse>> GetMBooksByWorkOrderId(int orderId);
        Task<PaginatedList<MBookHeaderResponse>> GetMBooksByUserIdPagination(int pageIndex, int pageSize, string search);
        Task<IResult<MBookDetailResponse>> GetMBooksById(int id);
        Task<List<MBItemStatusResponse>> GetCurrentMBItemsStatus(int id);
        Task<IResult> DeleteMeasurementBook(int id);
        Task<IResult<int>> CreateMeasurementBook(MBookRequest request);
        Task<IResult> UpdateMeasurementBook(int id, MBookRequest request);
        Task<IResult> PublishMeasurementBook(int id);
    }
}
