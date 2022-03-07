using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.RABillAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record CreateRABillCommand(RABillRequest Data) : IRequest<int>
    {
    }

    public class CreateRABillCommandQueryHandler : IRequestHandler<CreateRABillCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly IRABillService _billService;
        private readonly IMeasurementBookService _mBookService;

        public CreateRABillCommandQueryHandler(IAppDbContext context, IRABillService billService, IMeasurementBookService mBookService)
        {
            _context = context;
            _billService = billService;
            _mBookService = mBookService;
        }

        public async Task<int> Handle(CreateRABillCommand request, CancellationToken cancellationToken)
        {
            bool anyActiveRaBill = await _context.RABills.AnyAsync(
                p => p.Status == RABillStatus.CREATED || p.Status == RABillStatus.REVOKED);

            if(anyActiveRaBill)
            {
                throw new BadRequestException("Cannot create new RA Bill when there are exsiting unapproved RA Bill");
            }

            MeasurementBook mBook = await _context.MeasurementBooks
                .Include(p => p.Items)
                    .ThenInclude(i => i.WorkOrderItem)
                .Where(p => p.Id == request.Data.MeasurementBookId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (mBook == null)
            {
                throw new NotFoundException($"MeasurementBook does not exist with Id: {request.Data.MeasurementBookId}");
            }

            List<MBookItemQtyStatus> mBItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(mBook.Id);
            List<RAItemQtyStatus> raItemQtyStatuses = await _billService.GetRAItemQtyStatus(mBook.Id);

            var raBill = new RABill(
                title: request.Data.Title,
                billDate: (DateTime)request.Data.BillDate,
                mBookId: mBook.Id
            );

            foreach (var item in request.Data.Items)
            {
                var mBookItem = mBook.Items.FirstOrDefault(p => p.Id == item.MBookItemId);
                var mbItemQtyStatus = mBItemQtyStatuses.FirstOrDefault(p => p.MBookItemId == item.MBookItemId);
                var raItemQtyStatus = raItemQtyStatuses.FirstOrDefault(p => p.MBookItemId == item.MBookItemId);

                if (mBookItem == null)
                {
                    throw new NotFoundException($"MeasurementBook does not have line item with Id: {item.MBookItemId}");
                }

                raBill.AddLineItem(new RABillItem(
                    mbItemId: mBookItem.Id,
                    desc: mBookItem.WorkOrderItem.Description,
                    rate: mBookItem.WorkOrderItem.UnitRate,
                    acceptedMeasuredQty: mbItemQtyStatus != null ? mbItemQtyStatus.AcceptedMeasuredQty : 0,
                    tillLastRAQty: raItemQtyStatus != null ? raItemQtyStatus.ApprovedRAQty : 0,
                    currentRAQty: item.CurrentRAQty,
                    remarks: item.Remarks
                ));
            }

            _context.RABills.Add(raBill);
            await _context.SaveChangesAsync(cancellationToken);
            return raBill.Id;
        }

        private Exception BadRequestException(string v)
        {
            throw new NotImplementedException();
        }
    }
}
