using Application.Interfaces;
using AutoMapper;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MeasurementBookService : IMeasurementBookService
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public MeasurementBookService(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MBookItemApprovedQty>> GetMBItemsApprovedQty(int mBookId)
        {
            List<MBSheet> mbSheets = await _context.MBSheets
                 .Include(p => p.Items)
                 .Where(p => p.MeasurementBookId == mBookId)
                 .Where(p => p.Status == MBSheetStatus.ACCEPTED)
                 .AsNoTracking()
                 .ToListAsync();

            List<MBSheetItemResponse> items = new();
            List<MBookItemApprovedQty> mBookItemApprovedQties = new();

            foreach (var mbSheet in mbSheets)
            {
                items.AddRange(_mapper.Map<List<MBSheetItemResponse>>(mbSheet.Items));
            }

            List<int> mBookItemIds = items.Select(i => i.MBookItemId).Distinct().ToList();

            foreach (var mBookItemId in mBookItemIds)
            {
                float totalQuantity = items.Where(i => i.MBookItemId == mBookItemId)
                    .Aggregate((float)0, (acc, curr) => acc + curr.Quantity);

                var approvedQty = new MBookItemApprovedQty
                {
                    MBookItemId = mBookItemId,
                    TotalQuantity = totalQuantity
                };

                mBookItemApprovedQties.Add(approvedQty);
            }

            return mBookItemApprovedQties;
        }
    }
}

public class MBookItemApprovedQty
{
    public int MBookItemId { get; set; }
    public float TotalQuantity { get; set; }
}
