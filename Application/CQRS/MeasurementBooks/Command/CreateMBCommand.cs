using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MeasurementBookAggregate;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Requests;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record CreateMBCommand(CreateMBookRequest data) : IRequest<int>
    {
    }

    public class CreateWorkOrderCommandHandler : IRequestHandler<CreateMBCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWorkOrderItemService _itemService;

        public CreateWorkOrderCommandHandler(IAppDbContext context, 
            ICurrentUserService currentUserService,
            IWorkOrderItemService itemService)
        {
            _context = context;
            _itemService = itemService;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateMBCommand req, CancellationToken cancellationToken)
        {
            if (req.data.Items.Count == 0)
            {
                throw new BadRequestException("Measurement book line items cannot be empty");
            }

            var measurementBook = new MeasurementBook
            (
                workOrderId: req.data.WorkOrderId,
                measurementOfficer: req.data.MeasurementOfficer,
                validatingOfficer: req.data.ValidatingOfficer
            );

            foreach (var item in req.data.Items)
            {
                var isAvailable = await _itemService.IsBalanceQtyAvailable(item.WorkOrderItemId, item.Quantity);

                if (!isAvailable)
                {
                    throw new BadRequestException("Insufficient balance quantity for item " + item.ItemNo);
                }

                measurementBook.AddUpdateLineItem(item.WorkOrderItemId, item.Quantity);
            }

            _context.MeasurementBooks.Add(measurementBook);
            await _context.SaveChangesAsync(cancellationToken);

            return measurementBook.Id;
        }
    }
}