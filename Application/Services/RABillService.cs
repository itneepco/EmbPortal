using Application.Interfaces;
using AutoMapper;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RABillService : IRABillService
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public RABillService(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RAItemQtyStatus>> GetRAItemQtyStatus(int mBookId)
        {
            List<RABill> raBills = await _context.RABills
                 .Include(p => p.Items)
                 .Where(p => p.MeasurementBookId == mBookId && 
                       (p.Status == RABillStatus.APPROVED || p.Status == RABillStatus.POSTED))
                 .AsNoTracking()
                 .ToListAsync();

            List<RAItemQtyStatus> raItemQtyStatuses = new();

            List<RABillItem> raItems = new();
            foreach (var raBill in raBills)
            {
                raItems.AddRange(raBill.Items);
            }

            // select all possible work order item ids
            List<int> workOrderItemIds = raItems.Select(i => i.WorkOrderItemId).Distinct().ToList();

            foreach (var workOrderItemId in workOrderItemIds)
            {
                float approvedRAQty = raItems.Where(i => i.WorkOrderItemId == workOrderItemId)
                    .Aggregate((float)0, (acc, curr) => acc + curr.CurrentRAQty);

                var approvedQty = new RAItemQtyStatus
                {
                    WorkOrderItemId = workOrderItemId,
                    ApprovedRAQty = approvedRAQty
                };

                raItemQtyStatuses.Add(approvedQty);
            }

            return raItemQtyStatuses;
        }
    }
}

public class RAItemQtyStatus
{
    public int WorkOrderItemId { get; set; }
    public float ApprovedRAQty { get; set; }
}