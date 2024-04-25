using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Requests.MeasurementBooks;
using EmbPortal.Shared.Responses;
using EmbPortal.Shared.Responses.WorkOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<IResult<PurchaseOrder>> FetchPOFromSAP(string purchaseOrderNo);
        Task<IResult<int>> ReFetchPOFromSAP(long purchaseOrderNo);
        Task<PaginatedList<WorkOrderResponse>> GetUserWorkOrdersPagination(int pageIndex, int pageSize, string search);
        Task<IResult<WorkOrderDetailResponse>> GetWorkOrderById(int id);
        Task<List<WOItemStatusResponse>> GetWorkOrderItemStatus(int id);
        Task<string> ExportToExcelAsync();
        Task<IResult> DeleteWorkOrder(int id);
        Task<IResult<int>> CreateWorkOrder(WorkOrderRequest request);
        Task<IResult<List<PendingOrderItemResponse>>> GetPendingWorkOrderItems(int id);
        Task<IResult> ChangeEngineerIncharge(int id, ChangeOfficerRequest request);
    }
}
