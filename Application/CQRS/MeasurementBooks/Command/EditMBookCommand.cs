using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using EmbPortal.Shared.Requests;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EmbPortal.Shared.Enums;
using Domain.Entities.WorkOrderAggregate;

namespace Application.CQRS.MeasurementBooks.Command;

public record EditMBookCommand(int id, MBookRequest data) : IRequest
{
}

public class EditMBCommandHandler : IRequestHandler<EditMBookCommand>
{
    private readonly IAppDbContext _context;
    private readonly IWorkOrderService _orderService;

    public EditMBCommandHandler(IAppDbContext context, IWorkOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    public async Task Handle(EditMBookCommand req, CancellationToken cancellationToken)
    {
        var mBook = await _context.MeasurementBooks
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == req.id);

        if (mBook == null)
        {
            throw new NotFoundException(nameof(mBook), req.id);
        }

        if (mBook.Status == MBookStatus.PUBLISHED || mBook.Status == MBookStatus.COMPLETED)
        {
            throw new BadRequestException("Published measurement book cannot be updated");
        }

        mBook.SetWorkOrderId(req.data.WorkOrderId);
        //mBook.SetTitle(req.data.Title);
        mBook.SetMeasurementOfficer(req.data.MeasurementOfficer);
        mBook.SetValidatingOfficer(req.data.ValidatingOfficer);


        // Remove measurement book items that are not present in the new request object line items
        //----------------------------------- START --------------------------------------------//
        List<int> orderItemIds = mBook.Items.Select(p => p.WorkOrderItemId).ToList();
        foreach (var mBookItemId in orderItemIds)
        {
            var item = req.data.Items.FirstOrDefault(p => p.WorkOrderItemId == mBookItemId);
            if (item == null)
            {
                mBook.RemoveByOrderItemId(mBookItemId);
            }
        }
        //----------------------------------- END --------------------------------------------//

        // Retrieve work order with line items based on work order id
        var workOrder = await _orderService.GetWorkOrderWithItems(mBook.WorkOrderId);
        var existingMBookItems = await _orderService.GetAllExistingMBookItemsByOrderId(workOrder.Id);

        // Iterate over all request object items and process it
        foreach (var item in req.data.Items)
        {
            var mBookItem = mBook.Items.FirstOrDefault(p => p.WorkOrderItemId == item.WorkOrderItemId);
            if (mBookItem != null) continue; // Skip if line item already exists in Measurement Book

            var wOrderItem = workOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);
            
            if (wOrderItem == null)
            {
                throw new NotFoundException($"WorkOrder does not have LineItem with Id: {item.WorkOrderItemId}");
            }

            // check whether order item exists in other measurement books
            var mbItem = existingMBookItems.FirstOrDefault(p => p.WorkOrderItemId == wOrderItem.Id);
            if (mbItem != null)
            {
                throw new BadRequestException($"Line Item with Id: {item.WorkOrderItemId} is being used in some other Measurement Book");
            }

            mBook.AddUpdateLineItem(wOrderItem.Id);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
