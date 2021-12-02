using Application.Exceptions;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkOrderItemService : IWorkOrderItemService
    {
        private readonly IAppDbContext _context;
        public WorkOrderItemService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsBalanceQtyAvailable(int wOrderItemId, float quantity)
        {
            var workOrderItem = await _context.WorkOrderItems
                .Include(p => p.MBookItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == wOrderItemId);

            if (workOrderItem == null)
            {
                throw new NotFoundException(nameof(workOrderItem), wOrderItemId);
            }

            return workOrderItem.BalQuantity >= quantity ? true : false;
        }
    }
}
