using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Response;

namespace Application.Uoms.Query
{
   public class GetUomByIdQuery : IRequest<UomResponse>
    {
        public int Id { get; set; }
    }

    public class GetUomByIdQueryHandler : IRequestHandler<GetUomByIdQuery, UomResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetUomByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UomResponse> Handle(GetUomByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Uoms
            .ProjectTo<UomResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.Id);
        }
    }
}