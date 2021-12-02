using Domain.Entities.WorkOrderAggregate;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkOrderService
    {
        Task<WorkOrder> GetWorkOrderWithItems(int orderId);
    }
}
