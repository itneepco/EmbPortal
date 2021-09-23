using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Projects.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Query
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public int Id { get; set; }
    }

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetProjectByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Projects
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.Id);
        }
    }
}