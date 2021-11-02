using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Response;

namespace Application.Projects.Query
{
   public class GetProjectByIdQuery : IRequest<ProjectResponse>
    {
        public int Id { get; set; }
    }

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetProjectByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Projects
            .ProjectTo<ProjectResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.Id);
        }
    }
}