using Client.Models;
using EmbPortal.Shared.Responses;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<PaginatedList<WorkOrderResponse>> GetWorkOrdersByProjectPagination(int projectId, int pageIndex, int pageSize, string search);
        Task<PaginatedList<WorkOrderResponse>> GetUserWorkOrdersPagination(int pageIndex, int pageSize, string search);
        Task<IResult> DeleteWorkOrder(int id);
    }
}
