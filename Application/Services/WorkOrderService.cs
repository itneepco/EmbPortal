using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class WorkOrderService : IWorkOrderService
{
    private readonly IAppDbContext _context;
    public WorkOrderService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrder> GetWorkOrderWithItems(int orderId)
    {
        var workOrderItem = await _context.WorkOrders
            .Include(p => p.Items)                  
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == orderId);

        if (workOrderItem == null)
        {
            throw new NotFoundException(nameof(workOrderItem), orderId);
        }

        return workOrderItem;
    }

    public async Task<List<MBookItem>> GetAllExistingMBookItemsByOrderId(int orderId)
    {
        var mBooks = await _context.MeasurementBooks
            .Include(p => p.Items)
            .AsNoTracking()
            .Where(p => p.WorkOrderId == orderId)
            .ToListAsync();

        if (mBooks.Count == 0) return new List<MBookItem>();

        return (List<MBookItem>)mBooks.Select(p => p.Items)
            .Aggregate((acc, val) => Enumerable.Concat(acc, val).ToList());
    }
}
