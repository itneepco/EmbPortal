using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command;

public record CreateMBSheetCommand(MBSheetRequest Data) : IRequest<int>
{
}

public class CreateMBSheetCommandHandler : IRequestHandler<CreateMBSheetCommand, int>
{
    private readonly IAppDbContext _context;

    public CreateMBSheetCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMBSheetCommand request, CancellationToken cancellationToken)
    {
        var wOrderQuery = _context.WorkOrders.AsQueryable();
        var mBookQuery = _context.MeasurementBooks.AsQueryable();

        var query = from order in wOrderQuery
                    join mbook in mBookQuery
                    on order.Id equals mbook.WorkOrderId
                    select new { order, mbook };

        var result = await query
                        .Where(p => p.mbook.Id == request.Data.MeasurementBookId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

        if (result == null)
        {
            throw new NotFoundException($"MeasurementBook does not exist with Id: {request.Data.MeasurementBookId}");
        }

        if (result.mbook.Status == MBookStatus.CREATED)
        {
            throw new BadRequestException("Please publish Measurement Book before creating any MB Sheets");
        }

        if(result.order.OrderDate >= (DateTime)request.Data.MeasurementDate)
        {
            throw new BadRequestException("Measurement date cannot be earlier then PO Date");
        }

        var mbSheetCount = _context.MBSheets.Where(i => i.MeasurementBookId == result.mbook.Id).Count() + 1;
        var title = result.mbook.Title+"-Sheet-"+(mbSheetCount);
        var mbSheet = new MBSheet
        (
           title: title,
           measurementBookId: request.Data.MeasurementBookId,
           measurerEmpCode: result.mbook.MeasurerEmpCode,
           measurementDate: (DateTime)request.Data.MeasurementDate,
           validatorEmpCode: result.mbook.ValidatorEmpCode,
           eicEmpCode: result.mbook.EicEmpCode
        );

        _context.MBSheets.Add(mbSheet);
        await _context.SaveChangesAsync(cancellationToken);
        return mbSheet.Id;
    }
}
