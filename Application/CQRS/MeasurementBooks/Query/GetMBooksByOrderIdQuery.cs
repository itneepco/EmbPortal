using Application.Interfaces;
using AutoMapper;
using MediatR;
using EmbPortal.Shared.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Mappings;

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetMBooksByOrderIdQuery(int workOrderId) : IRequest<List<MBookResponse>>
    {
    }

    public class GetMBooksByOrderIdQueryHandler : IRequestHandler<GetMBooksByOrderIdQuery, List<MBookResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMeasurementBookService _mBookService;

        public GetMBooksByOrderIdQueryHandler(IAppDbContext context, IMapper mapper, IMeasurementBookService mBookService)
        {
            _mapper = mapper;
            _context = context;
            _mBookService = mBookService;
        }

        public async Task<List<MBookResponse>> Handle(GetMBooksByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var mBooks = await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                .Where(p => p.WorkOrderId == request.workOrderId)
                .AsNoTracking()
                .ProjectToListAsync<MBookResponse>(_mapper.ConfigurationProvider);

            // Calculating the Item Quantity Status for each measurement book items --- START ---
            foreach (var mBook in mBooks)
            {
                List<MBookItemQtyStatus> itemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(mBook.Id);

                foreach (var item in mBook.Items)
                {
                    var itemQtyStatus = itemQtyStatuses.Find(i => i.MBookItemId == item.Id);
                    item.AcceptedMeasuredQty = itemQtyStatus != null ? itemQtyStatus.AcceptedMeasuredQty : 0;
                    item.CumulativeMeasuredQty = itemQtyStatus != null ? itemQtyStatus.CumulativeMeasuredQty : 0;
                }
            }
            // --- END ---

            return mBooks;
        }
    }
}
