using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Uoms.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Uoms.Query
{
    public class GetUomsQuery : IRequest<IReadOnlyList<UomDto>>
    {

    }

    public class GetUomsQueryHandler : IRequestHandler<GetUomsQuery, IReadOnlyList<UomDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetUomsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<UomDto>> Handle(GetUomsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Uoms
                .OrderBy(p => p.Name)
                .ProjectTo<UomDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}