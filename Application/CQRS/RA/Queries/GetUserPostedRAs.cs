using Application.Interfaces;
using EmbPortal.Shared.Responses.RA;
using Infrastructure.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EmbPortal.Shared.Enums;

namespace Application.CQRS.RA.Queries;

public record GetUserPostedRAs : IRequest<List<RAResponse>>;

public class GetUserPostedRAsHandler : IRequestHandler<GetUserPostedRAs, List<RAResponse>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserPostedRAsHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<RAResponse>> Handle(GetUserPostedRAs request, CancellationToken cancellationToken)
    {
        var empCode = _currentUserService.EmployeeCode;

        var raResponses = await _context.RAHeaders
                            .Include(x => x.Items)
                            .Include(y => y.Deductions)
                            .Where(p => p.EicEmpCode == empCode && p.Status == RAStatus.Posted)
                            .Select(q => new RAResponse
                            {
                                Id = q.Id,
                                WorkOrderId = q.WorkOrderId,
                                Title = q.Title,
                                Status = q.Status,
                                BillDate = q.BillDate,
                                RAAmount = q.Items.Sum(i => i.UnitRate * (decimal)i.CurrentRAQty),
                                Deduction = q.Deductions.Sum(d => d.Amount)
                            }).
                            ToListAsync(cancellationToken);

        return raResponses;
    }
}