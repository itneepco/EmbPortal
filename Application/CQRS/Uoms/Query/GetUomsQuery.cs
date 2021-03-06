using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Responses;
using Application.Mappings;

namespace Application.Uoms.Query
{
    public class GetUomsQuery : IRequest<IReadOnlyList<UomResponse>>
    {
    }

    public class GetUomsQueryHandler : IRequestHandler<GetUomsQuery, IReadOnlyList<UomResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetUomsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<UomResponse>> Handle(GetUomsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Uoms
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ProjectToListAsync<UomResponse>(_mapper.ConfigurationProvider);
        }

    }
}