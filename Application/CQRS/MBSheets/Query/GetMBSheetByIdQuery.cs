using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.MBSheetAggregate;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Query;

public record GetMBSheetByIdQuery(int Id) : IRequest<MBSheetResponse>
{
}

public class GetMBSheetByIdQueryHandler : IRequestHandler<GetMBSheetByIdQuery, MBSheetResponse>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetMBSheetByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MBSheetResponse> Handle(GetMBSheetByIdQuery request, CancellationToken cancellationToken)
    {
        var userQuery = _context.AppUsers.AsQueryable();
        var orderQuery = _context.WorkOrders.Include(p => p.Items).AsQueryable();
        var msheetQuery = _context.MBSheets
                            .Include(p => p.Items).ThenInclude(p => p.Attachments)
                            .Include(p => p.Items).ThenInclude(p => p.Measurements)
                            .AsQueryable();

        var query = from msheet in msheetQuery
                    join wOrder in orderQuery on msheet.WorkOrderId equals wOrder.Id
                    join measurer in userQuery on msheet.MeasurerEmpCode equals measurer.UserName
                    join validator in userQuery on msheet.ValidatorEmpCode equals validator.UserName
                    join eic in userQuery on msheet.EicEmpCode equals eic.UserName
                    select new { msheet, wOrder, measurer, validator, eic };

        var result = await query
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.msheet.Id == request.Id);

        if (result == null)
        {
            throw new NotFoundException(nameof(MBSheet), request.Id);
        }

        var mbSheet = _mapper.Map<MBSheetResponse>(result.msheet);

        mbSheet.MeasurerName = result.measurer.DisplayName;
        mbSheet.ValidatorName = result.validator.DisplayName;
        mbSheet.EicName = result.eic.DisplayName;

        foreach (var item in mbSheet.Items)
        {
            var wOrderItem = result.wOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);
            
            if (wOrderItem == null)
            {
                throw new NotFoundException($"Item does not exists with Id: {item.WorkOrderItemId}");
            }

            var response = _mapper.Map<MBSheetItemResponse>(item);

            response.ServiceNo = wOrderItem.ServiceNo;
            response.Uom = wOrderItem.Uom;
            response.UnitRate = wOrderItem.UnitRate;
            response.ShortServiceDesc = wOrderItem.ShortServiceDesc;
        }

        return mbSheet;
    }
}
