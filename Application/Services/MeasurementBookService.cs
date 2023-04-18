using Application.Interfaces;
using AutoMapper;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class MeasurementBookService : IMeasurementBookService
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public MeasurementBookService(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MBookItemQtyStatus>> GetMBItemsQtyStatus(int mBookId)
    {
        List<MBSheet> mbSheets = await _context.MBSheets
             .Include(p => p.Items)
             .Where(p => p.MeasurementBookId == mBookId)
             .AsNoTracking()
             .ToListAsync();

        List<MBookItemQtyStatus> mBookItemQtyStatuses = new();

        // for mb sheet items
        List<MBSheetItem> mbSheetItems = new();
        foreach (var mbSheet in mbSheets)
        {
            mbSheetItems.AddRange(mbSheet.Items);
        }

        // for accepted mb sheet items
        List<MBSheetItem> acceptedMBSheetItems = new();
        foreach (var mbSheet in mbSheets.Where(p => p.Status == MBSheetStatus.ACCEPTED))
        {
            acceptedMBSheetItems.AddRange(mbSheet.Items);
        }

        // select all possible work order item ids
        List<int> workOrderItemIds = mbSheetItems.Select(i => i.WorkOrderItemId).Distinct().ToList();

        foreach (var workOrderItemId in workOrderItemIds)
        {
            float totalMeasuredQty = mbSheetItems.Where(i => i.WorkOrderItemId == workOrderItemId)
                .Aggregate((float)0, (acc, curr) => acc + curr.MeasuredQuantity);

            float acceptedMeasuredQty = acceptedMBSheetItems.Where(i => i.WorkOrderItemId == workOrderItemId)
                .Aggregate((float)0, (acc, curr) => acc + curr.MeasuredQuantity);

            var approvedQty = new MBookItemQtyStatus
            {
                WorkOrderItemId = workOrderItemId,
                TotalMeasuredQty = totalMeasuredQty,
                AcceptedMeasuredQty = acceptedMeasuredQty
            };

            mBookItemQtyStatuses.Add(approvedQty);
        }

        return mBookItemQtyStatuses;
    }
}

public class MBookItemQtyStatus
{
    public int WorkOrderItemId { get; set; }
    public float TotalMeasuredQty { get; set; }
    public float AcceptedMeasuredQty { get; set; }
}
