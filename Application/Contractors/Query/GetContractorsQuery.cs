using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contractors.Response;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contractors.Query
{
    public record GetContractorsQuery : IRequest<IReadOnlyList<ContractorDto>>
    {
        
    }

    public class GetContractorsQueryHandler : IRequestHandler<GetContractorsQuery, IReadOnlyList<ContractorDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetContractorsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ContractorDto>> Handle(GetContractorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Contractors
                .OrderBy(p => p.Name)
                .ProjectTo<ContractorDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
        }

    }
    
}