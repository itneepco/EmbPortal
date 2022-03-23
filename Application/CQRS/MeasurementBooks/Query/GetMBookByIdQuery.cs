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
        private readonly IRABillService _raBillService;

        private Expression<Func<MeasurementBook, bool>> Criteria { set; get; }

        public GetMBookByIdQueryHandler(IMapper mapper, IAppDbContext context, ICurrentUserService userService, IMeasurementBookService mBookService, IRABillService raBillService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
            _mBookService = mBookService;
            _raBillService = raBillService;
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

            var mBook = await query.ProjectTo<MBookDetailResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (mBook == null)
            {
                throw new NotFoundException(nameof(MeasurementBook), request.Id);
            }

            // Fetch the MB items status
            List<MBookItemQtyStatus> mbItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(mBook.Id);

            // Fetch the cumulative RA items quantity
            List<RAItemQtyStatus> raItemQtyStatuses = await _raBillService.GetRAItemQtyStatus(mBook.Id);

            foreach (var item in mBook.Items)
            {
                var mbItemQtyStatus = mbItemQtyStatuses.Find(i => i.MBookItemId == item.Id);
                var raItemQtyStatus = raItemQtyStatuses.Find(i => i.MBookItemId == item.Id);

                item.AcceptedMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.AcceptedMeasuredQty : 0;
                item.CumulativeMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.CumulativeMeasuredQty : 0;
                item.TillLastRAQty = raItemQtyStatus != null ? raItemQtyStatus.ApprovedRAQty : 0;
            }
            // --- END ---

            return mBook;
        }
    }
}
