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
    public class GetUomByIdQuery : IRequest<UomDto>
    {
        public int Id { get; set; }
    }

    public class GetUomByIdQueryHandler : IRequestHandler<GetUomByIdQuery, UomDto>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetUomByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UomDto> Handle(GetUomByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Uoms
            .ProjectTo<UomDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.Id);
        }
    }
}