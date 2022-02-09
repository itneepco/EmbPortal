using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetMBookByIdQuery(int Id) : IRequest<MBookDetailResponse>
    {
    }

    public class GetMBookByIdQueryHandler : IRequestHandler<GetMBookByIdQuery, MBookDetailResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;
        private readonly IMeasurementBookService _mBookService;

        private Expression<Func<MeasurementBook, bool>> Criteria { set; get; }

        public GetMBookByIdQueryHandler(IMapper mapper, IAppDbContext context, ICurrentUserService userService, IMeasurementBookService mBookService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
            _mBookService = mBookService;
        }

        public async Task<MBookDetailResponse> Handle(GetMBookByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MeasurementBooks
                .Include(p => p.WorkOrder)
                .Include(p => p.WorkOrder.Contractor)
                .Include(p => p.WorkOrder.Project)
                .Include(p => p.Items)
                .AsQueryable();

            Criteria = (m =>
                m.MeasurementOfficer == _userService.EmployeeCode ||
                m.ValidatingOfficer == _userService.EmployeeCode ||
                m.WorkOrder.EngineerInCharge == _userService.EmployeeCode
            );

            query = query.Where(Criteria);

            var mbook = await query.ProjectTo<MBookDetailResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (mbook == null)
            {
                throw new NotFoundException(nameof(mbook), request.Id);
            }

            // Calculating the approved quantity for each measurement book items --- START ---
            List<MBookItemApprovedQty> approvedItemQties = await _mBookService.GetMBItemsApprovedQty(mbook.Id);
            
            foreach (var item in mbook.Items)
            {
                var approvedItemQty = approvedItemQties.Find(i => i.MBookItemId == item.Id);
                item.ApprovedQuantity = approvedItemQty != null ? approvedItemQty.TotalQuantity : 0; 
            }
            // --- END ---

            return mbook;
        }
    }
}
