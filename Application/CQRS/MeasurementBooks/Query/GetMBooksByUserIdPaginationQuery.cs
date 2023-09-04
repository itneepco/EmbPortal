using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query;

public record GetMBooksByUserIdPaginationQuery(PagedRequest Data) : IRequest<PaginatedList<MBookHeaderResponse>>
{
}

public class GetMBooksByUserIdPaginationQueryHandler : IRequestHandler<GetMBooksByUserIdPaginationQuery, PaginatedList<MBookHeaderResponse>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    private Expression<Func<MBookHeaderResponse, bool>> Criteria { set; get; }

    public GetMBooksByUserIdPaginationQueryHandler(IMapper mapper, IAppDbContext context, ICurrentUserService userService)
    {
        _mapper = mapper;
        _context = context;
        _userService = userService;
    }

    public async Task<PaginatedList<MBookHeaderResponse>> Handle(GetMBooksByUserIdPaginationQuery request, CancellationToken cancellationToken)
    {
        var empCode = _userService.EmployeeCode;
        var mBookQuery = _context.MeasurementBooks               
                        .Where(p => p.Status != MBookStatus.CREATED &&
                        (p.MeasurerEmpCode == empCode || p.ValidatorEmpCode == empCode || p.EicEmpCode == empCode))
                        .AsQueryable();

        var woQuery = _context.WorkOrders.AsQueryable();
        var query = from mBook in mBookQuery
                    join order in woQuery
                    on mBook.WorkOrderId equals order.Id
                    select new MBookHeaderResponse
                    { 
                        Id = mBook.Id,
                        Title = mBook.Title,
                        OrderNo = order.OrderNo.ToString(),
                        OrderDate = order.OrderDate,
                        Contractor = order.Contractor,
                        Status = mBook.Status.ToString()                            
                    };

        
        if (!string.IsNullOrEmpty(request.Data.Search)) // Query based on the filter on the measurement book
        {
            Criteria = (m =>
                m.Title.ToLower().Contains(request.Data.Search.ToLower()) ||
                m.OrderNo.ToString().Contains(request.Data.Search.ToLower()) ||
                m.Contractor.ToLower().Contains(request.Data.Search.ToLower())
            );
            query = query.Where(Criteria);
        }

        if (request.Data.Status > 1) // Query based on the status of the measurement book
        {
            Criteria = m =>m.Status == ((MBookStatus)request.Data.Status).ToString();
            query = query.Where(Criteria);
        }

        

        return await query.OrderBy(p => p.OrderDate)                
            .AsNoTracking()
            .PaginatedListAsync(request.Data.PageNumber, request.Data.PageSize);
    }
}
