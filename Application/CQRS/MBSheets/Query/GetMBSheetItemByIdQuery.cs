using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Query;

public record GetMBSheetItemByIdQuery(int MBSheetId, int MBSheetItemId) : IRequest<MBSheetItemResponse>;

public class GetMBSheetItemByIdQueryHandler : IRequestHandler<GetMBSheetItemByIdQuery, MBSheetItemResponse>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetMBSheetItemByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MBSheetItemResponse> Handle(GetMBSheetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var mbSheet = await _context.MBSheets
            .Include(p => p.Items)
            .ThenInclude(i => i.Measurements)
            .FirstOrDefaultAsync(p => p.Id == request.MBSheetId);

        if (mbSheet == null)
        {
            throw new NotFoundException($"MB Sheet does not exists with Id: {request.MBSheetId}");
        }

        var response = mbSheet.Items.FirstOrDefault(p => p.Id == request.MBSheetItemId);

        return _mapper.Map<MBSheetItemResponse>(response);
    }
}

