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

        private Expression<Func<MeasurementBook, bool>> Criteria { set; get; }

        public GetMBookByIdQueryHandler(IMapper mapper, IAppDbContext context, ICurrentUserService userService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
        }

        public async Task<MBookDetailResponse> Handle(GetMBookByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MeasurementBooks.AsQueryable();

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

            return mbook;
        }
    }
}
