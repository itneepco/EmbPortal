using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IAppDbContext _context;
        public WorkOrderService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkOrder> GetWorkOrderWithItems(int orderId)
        {
            //var workOrderItem = await _context.WorkOrders
            //    .Include(p => p.Items)
            //        .ThenInclude(i => i.MBookItem)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(p => p.Id == orderId);

            //if (workOrderItem == null)
            //{
            //    throw new NotFoundException(nameof(workOrderItem), orderId);
            //}

            return null;
        }
    }
}
