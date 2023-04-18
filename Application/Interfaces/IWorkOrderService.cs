using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.WorkOrderAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IWorkOrderService
{
    Task<WorkOrder> GetWorkOrderWithItems(int orderId);
    Task<List<MBookItem>> GetAllExistingMBookItemsByOrderId(int orderId);
}
