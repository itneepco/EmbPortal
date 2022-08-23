using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.WorkOrderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Command
{
    public record CreateWorkOrderItemCommand(int workOrderId, WorkOrderItemRequest data) : IRequest<int>
    {
    }

    public class CreateWorkOrderItemCommandHandler : IRequestHandler<CreateWorkOrderItemCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateWorkOrderItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateWorkOrderItemCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == request.workOrderId);

            if (workOrder == null)
            {
                throw new NotFoundException(nameof(workOrder), request.workOrderId);
            }


            workOrder.AddUpdateLineItem(
                itemNo: request.data.ItemNo,
                pacakageNo: request.data.PackageNo,
                itemDesc: request.data.ItemDescription,
                subItemNo: request.data.SubItemNo,
                subItemPacakageNo: request.data.SubItemPackageNo,
                serviceNo: request.data.ServiceNo,
                shortServiceDesc: request.data.ShortServiceDesc,
                longServiceDesc: request.data.LongServiceDesc,
                uomId: request.data.UomId,
                unitRate: request.data.UnitRate,
                poQuantity: request.data.PoQuantity
            );

            await _context.SaveChangesAsync(cancellationToken);

            var workOrderitem = workOrder.Items.FirstOrDefault(p => p.ServiceNo == request.data.ServiceNo);
            return workOrderitem.Id;
        }
    }

}
