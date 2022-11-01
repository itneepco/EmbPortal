using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MeasurementBookAggregate;
using MediatR;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Enums;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record CreateMBookCommand(MBookRequest data) : IRequest<int>
    {
    }

    public class CreateWorkOrderCommandHandler : IRequestHandler<CreateMBookCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly IWorkOrderService _orderService;

        public CreateWorkOrderCommandHandler(IAppDbContext context, 
            IWorkOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<int> Handle(CreateMBookCommand req, CancellationToken cancellationToken)
        {
            if (req.data.Items.Count == 0)
            {
                throw new BadRequestException("Measurement book line items cannot be empty");
            }

            var workOrder = await _orderService.GetWorkOrderWithItems(req.data.WorkOrderId);

            if(workOrder.Status == WorkOrderStatus.CREATED)
            {
                throw new BadRequestException("Please publish Work Order before creating any Measurement Books");
            }
            
            var mbCount = _context.MeasurementBooks.Where( i => i.WorkOrderId == req.data.WorkOrderId ).Count()+1;            
            var title = workOrder.OrderNo +"-MB-"+mbCount;
            var measurementBook = new MeasurementBook
            (
                workOrderId: req.data.WorkOrderId,
                title: title,
                measurementOfficer: req.data.MeasurementOfficer,
                validatingOfficer: req.data.ValidatingOfficer
            );

            foreach (var item in req.data.Items)
            {
                var workOrderItem = workOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);

                if (workOrderItem == null)
                {
                    throw new NotFoundException($"WorkOrder does not have LineItem with Id: {item.WorkOrderItemId}");
                }
                if (workOrderItem.MBookItem != null)
                {
                    throw new BadRequestException($"LineItem with Id: {item.WorkOrderItemId} is being used in some other Measurement Book");
                }
                measurementBook.AddUpdateLineItem(workOrderItem.Id);
            }

            _context.MeasurementBooks.Add(measurementBook);
            await _context.SaveChangesAsync(cancellationToken);

            return measurementBook.Id;
        }
    }
}