using Client.Models;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<PaginatedList<WorkOrderResponse>> GetWorkOrdersByProjectPagination(int projectId, int pageIndex, int pageSize, string search);
        Task<PaginatedList<WorkOrderResponse>> GetUserWorkOrdersPagination(int pageIndex, int pageSize, string search);
        Task<WorkOrderDetailResponse> GetWorkOrderById(int id);
        Task<IResult> DeleteWorkOrder(int id);
        Task<IResult<int>> CreateWorkOrder(WorkOrderRequest request);
        Task<IResult> UpdateWorkOrder(int id, WorkOrderRequest request);
    }
}
