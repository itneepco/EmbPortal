using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RABills.Commands
{
    public record PostRABillToSapCommand(int RABillId) : IRequest
    {
    }
    public class PostRABillToSapCommandHandler : IRequestHandler<PostRABillToSapCommand>
    {
        private readonly IAppDbContext _context;
        public PostRABillToSapCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostRABillToSapCommand request, CancellationToken cancellationToken)
        {
            var raBill = await _context.RABills
                .Include(i => i.Items)
                .Include(p => p.MeasurementBook)
                .ThenInclude (m => m.WorkOrder)
                .FirstOrDefaultAsync(p => p.Id == request.RABillId && p.Status == RABillStatus.APPROVED);
            if(raBill == null)
            {
                throw new NotFoundException("RA Bill Not found for posting");
            }

            var sapSEHeader = new SapSEHeader
            {
                OrderNo = raBill.MeasurementBook.WorkOrder.OrderNo,
                ItemNo = raBill.Items.First().ItemNo,
                PackageNo = raBill.Items.First().PackageNo,
                Details = new()
            };
            foreach (var item in raBill.Items)
            {
                var sapSEItem = new SapSEItem
                {
                   SubItemNo = item.SubItemNo,
                   SubItemPackageNo = item.SubItemPackageNo,
                   Quantity = item.CurrentRAQty,
                };
                sapSEHeader.Details.Add(sapSEItem);
            }
            //---todo post to sap.

            raBill.MarkAsPosted();

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
            
        }
    }
    class SapSEHeader
    {
        public long OrderNo { get; set; }
        public long ItemNo { get; set; }
        public string PackageNo { get; set; }
        public List<SapSEItem> Details { get; set; }
    }
    class SapSEItem
    {
        public long SubItemNo { get; set; }
        public string SubItemPackageNo { get; set; }
        public float Quantity { get; set; }
    }

}

