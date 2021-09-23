using System.Collections.Generic;
using System.Linq;
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
    public class GetProjectsQuery : IRequest<IReadOnlyList<ProjectDto>>
    {

    }

    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IReadOnlyList<ProjectDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetProjectsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Projects
            .OrderBy(p => p.Name)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}