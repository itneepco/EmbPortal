using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Responses;

namespace Application.Contractors.Query
{
   public record GetContractorByIdQuery(int id) : IRequest<ContractorResponse>
    {
        
    }

    public class GetContractorByIdQueryHandler : IRequestHandler<GetContractorByIdQuery, ContractorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetContractorByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractorResponse> Handle(GetContractorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Uoms
            .ProjectTo<ContractorResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.id);
        }
    }
}