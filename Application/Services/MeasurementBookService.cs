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

        public async Task<List<MBookItemQtyStatus>> GetMBItemsQtyStatus(int mBookId)
        {
            List<MBSheet> mbSheets = await _context.MBSheets
                 .Include(p => p.Items)
                 .Where(p => p.MeasurementBookId == mBookId)
                 .AsNoTracking()
                 .ToListAsync();

            List<MBookItemQtyStatus> mBookItemQtyStatuses = new();

            // for mb sheet items
            List<MBSheetItemResponse> mbSheetItems = new();
            foreach (var mbSheet in mbSheets)
            {
                mbSheetItems.AddRange(_mapper.Map<List<MBSheetItemResponse>>(mbSheet.Items));
            }

            // for accepted mb sheet items
            List<MBSheetItemResponse> acceptedMBSheetItems = new();
            foreach (var mbSheet in mbSheets.Where(p => p.Status == MBSheetStatus.ACCEPTED ))
            {
                acceptedMBSheetItems.AddRange(_mapper.Map<List<MBSheetItemResponse>>(mbSheet.Items));
            }

            // select all possible measurement book item ids
            List<int> mBookItemIds = mbSheetItems.Select(i => i.MBookItemId).Distinct().ToList();

            foreach (var mBookItemId in mBookItemIds)
            {
                float cumulativeMeasuredQty = mbSheetItems.Where(i => i.MBookItemId == mBookItemId)
                    .Aggregate((float)0, (acc, curr) => acc + curr.Quantity);

                float acceptedMeasuredQty = acceptedMBSheetItems.Where(i => i.MBookItemId == mBookItemId)
                    .Aggregate((float)0, (acc, curr) => acc + curr.Quantity);

                var approvedQty = new MBookItemQtyStatus
                {
                    MBookItemId = mBookItemId,
                    CumulativeMeasuredQty = cumulativeMeasuredQty,
                    AcceptedMeasuredQty = acceptedMeasuredQty
                };

                mBookItemQtyStatuses.Add(approvedQty);
            }

            return mBookItemQtyStatuses;
        }
    }
}

public class MBookItemQtyStatus
{
    public int MBookItemId { get; set; }
    public float AcceptedMeasuredQty { get; set; }
    public float CumulativeMeasuredQty { get; set; }
}
