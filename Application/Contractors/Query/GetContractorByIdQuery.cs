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
    public record GetContractorByIdQuery(int id) : IRequest<ContractorDto>
    {
        
    }

    public class GetContractorByIdQueryHandler : IRequestHandler<GetContractorByIdQuery, ContractorDto>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetContractorByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractorDto> Handle(GetContractorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Uoms
            .ProjectTo<ContractorDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.id);
        }
    }
}