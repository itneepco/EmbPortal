using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetCurrentMBookItemsStatusQuery(int MBookId) : IRequest<List<MBItemStatusResponse>>
    {
    }

    public class GetCurrentMBookItemsStatusQueryHandler : IRequestHandler<GetCurrentMBookItemsStatusQuery, List<MBItemStatusResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMeasurementBookService _mBookService;
        private readonly IRABillService _raBillService;

        public GetCurrentMBookItemsStatusQueryHandler(IAppDbContext context, IMeasurementBookService mBookService, IRABillService raBillService)
        {
            _context = context;
            _mBookService = mBookService;
            _raBillService = raBillService;
        }

        public async Task<List<MBItemStatusResponse>> Handle(GetCurrentMBookItemsStatusQuery request, CancellationToken cancellationToken)
        {
            var mBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                        .ThenInclude(i => i.Uom)
                .FirstOrDefaultAsync(p => p.Id == request.MBookId);

            if (mBook == null)
            {
                throw new NotFoundException(nameof(mBook), request.MBookId);
            }

            // Fetch the MB items status
            List<MBookItemQtyStatus> mbItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(mBook.Id);

            // Fetch the cumulative RA items quantity
            List<RAItemQtyStatus> raItemQtyStatuses = await _raBillService.GetRAItemQtyStatus(mBook.Id);

            List<MBItemStatusResponse> itemStatusResponses = new();
            foreach (var item in mBook.Items)
            {
                var mbItemQtyStatus = mbItemQtyStatuses.Find(i => i.MBookItemId == item.Id);
                var raItemQtyStatus = raItemQtyStatuses.Find(i => i.MBookItemId == item.Id);

                itemStatusResponses.Add(new MBItemStatusResponse
                {
                    MBookItemId = item.Id,
                    ItemDescription = item.WorkOrderItem.ShortServiceDesc,
                    UnitRate = item.WorkOrderItem.UnitRate,
                    Dimension = (int)item.WorkOrderItem.Uom.Dimension,
                    Uom = item.WorkOrderItem.Uom.Name,
                    PoQuantity = item.WorkOrderItem.PoQuantity,
                    CumulativeMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.CumulativeMeasuredQty : 0,
                    AcceptedMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.AcceptedMeasuredQty : 0,
                    TillLastRAQty = raItemQtyStatus != null ? raItemQtyStatus.ApprovedRAQty : 0
                });
            }

            return itemStatusResponses;
        }
    }
}
