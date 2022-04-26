using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<PaginatedList<WorkOrderResponse>> GetWorkOrdersByProjectPagination(int projectId, int pageIndex, int pageSize, string search);
        Task<PaginatedList<WorkOrderResponse>> GetUserWorkOrdersPagination(int pageIndex, int pageSize, string search);
        Task<IResult<WorkOrderDetailResponse>> GetWorkOrderById(int id);
        Task<string> ExportToExcelAsync();
        Task<IResult> UploadWorkOrderItemsAsync(int workOrderId, FileUploadRequest content);
        Task<IResult> DeleteWorkOrder(int id);
        Task<IResult<int>> CreateWorkOrder(WorkOrderRequest request);
        Task<IResult> UpdateWorkOrder(int id, WorkOrderRequest request);
        Task<IResult> PublishWorkOrder(int id);
        Task<IResult> PublishWorkOrderItem(int workOrderId, int orderItemId);
        Task<IResult<int>> CreateWorkOrderItem(int id, WorkOrderItemRequest request);
        Task<IResult> UpdateWorkOrderItem(int id, int itemId, WorkOrderItemRequest request);
        Task<IResult> DeleteWorkOrderItem(int id, int itemId);
        Task<IResult<List<PendingOrderItemResponse>>> GetPendingWorkOrderItems(int id);
    }
}
