using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record EditRABillCommand(int RaBillId, RABillRequest Data) : IRequest
    {
    }

    public class EditRABillCommandHandler : IRequestHandler<EditRABillCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IRABillService _billService;
        private readonly IMeasurementBookService _mBookService;

        public EditRABillCommandHandler(IAppDbContext context, IRABillService billService, IMeasurementBookService mBookService)
        {
            _context = context;
            _billService = billService;
            _mBookService = mBookService;
        }
        public async Task<Unit> Handle(EditRABillCommand request, CancellationToken cancellationToken)
        {
            RABill raBill = await _context.RABills
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.RaBillId);

            if (raBill == null)
            {
                throw new NotFoundException(nameof(RABill), request.RaBillId);
            }

            raBill.SetTitle(request.Data.Title);
            raBill.SetBillDate((DateTime)request.Data.BillDate);

            //Fetch the line item status from db
            List<MBookItemQtyStatus> mBItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(raBill.MeasurementBookId);
            List<RAItemQtyStatus> raItemQtyStatuses = await _billService.GetRAItemQtyStatus(raBill.MeasurementBookId);

            // iterate over all ra items and update accordingly 
            foreach (var item in raBill.Items)
            {
                var mbItemQtyStatus = mBItemQtyStatuses.FirstOrDefault(p => p.MBookItemId == item.MBookItemId);
                var raItemQtyStatus = raItemQtyStatuses.FirstOrDefault(p => p.MBookItemId == item.MBookItemId);
                var raItemRequest = request.Data.Items.Find(p => p.MBookItemId == item.MBookItemId);

                item.SetAcceptedMeasuredQty(mbItemQtyStatus.AcceptedMeasuredQty);
                item.SetTillLastRAQty(raItemQtyStatus.ApprovedRAQty);
                item.SetCurrentRAQty(raItemRequest.CurrentRAQty);
                item.SetRemarks(raItemRequest.Remarks);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
