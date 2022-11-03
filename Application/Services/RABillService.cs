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

            List<RABillItemResponse> raItems = new();
            foreach (var raBill in raBills)
            {
                raItems.AddRange(_mapper.Map<List<RABillItemResponse>>(raBill.Items));
            }

            // select all possible measurement book item ids
            List<int> mBookItemIds = raItems.Select(i => i.MBookItemId).Distinct().ToList();

            foreach (var mBookItemId in mBookItemIds)
            {
                float approvedRAQty = raItems.Where(i => i.MBookItemId == mBookItemId)
                    .Aggregate((float)0, (acc, curr) => acc + curr.CurrentRAQty);

                var approvedQty = new RAItemQtyStatus
                {
                    MBookItemId = mBookItemId,
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
    public int MBookItemId { get; set; }
    public float ApprovedRAQty { get; set; }
}