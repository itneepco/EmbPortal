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

namespace Application.Contractors.Query
{
    public record GetContractorsQuery : IRequest<IReadOnlyList<ContractorResponse>>
    {
        
    }

    public class GetContractorsQueryHandler : IRequestHandler<GetContractorsQuery, IReadOnlyList<ContractorResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetContractorsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ContractorResponse>> Handle(GetContractorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Contractors
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ProjectToListAsync<ContractorResponse>(_mapper.ConfigurationProvider);
        }

    }
    
}