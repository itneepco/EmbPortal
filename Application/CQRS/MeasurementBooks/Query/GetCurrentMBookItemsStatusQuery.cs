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

        public GetCurrentMBookItemsStatusQueryHandler(IAppDbContext context, IMeasurementBookService mBookService)
        {
            _context = context;
            _mBookService = mBookService;
        }

        public async Task<List<MBItemStatusResponse>> Handle(GetCurrentMBookItemsStatusQuery request, CancellationToken cancellationToken)
        {
            var mBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                .FirstOrDefaultAsync(p => p.Id == request.MBookId);

            if (mBook == null)
            {
                throw new NotFoundException(nameof(mBook), request.MBookId);
            }

            // Fetch the MB items status
            List<MBookItemQtyStatus> itemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(mBook.Id);

            List<MBItemStatusResponse> itemStatusResponses = new();
            foreach (var item in mBook.Items)
            {
                var itemQtyStatus = itemQtyStatuses.Find(i => i.MBookItemId == item.Id);

                itemStatusResponses.Add(new MBItemStatusResponse
                {
                    MBookItemId = item.Id,
                    ItemDescription = item.WorkOrderItem.Description,
                    UnitRate = item.WorkOrderItem.UnitRate,
                    AcceptedMeasuredQty = itemQtyStatus != null ? itemQtyStatus.AcceptedMeasuredQty : 0,
                    TillLastRAQty = 0 // TODO - Calculate the Till Last RA Quantity from the database
                });
            }

            return itemStatusResponses;
        }
    }
}
