using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Responses;

namespace Application.Uoms.Query
{
    public record GetUomByIdQuery(int id) : IRequest<UomResponse>
    {
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
            var uom = await _context.Uoms
                .ProjectTo<UomResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.id);

            if (uom == null)
            {
                throw new NotFoundException(nameof(uom), request.id);
            }

            return uom;
        }
    }
}