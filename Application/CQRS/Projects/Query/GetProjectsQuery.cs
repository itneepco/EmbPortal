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

namespace Application.Projects.Query
{
    public class GetProjectsQuery : IRequest<IReadOnlyList<ProjectResponse>>
    {

    }

    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IReadOnlyList<ProjectResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppDbContext _context;
        public GetProjectsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProjectResponse>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Projects
            .OrderBy(p => p.Name)
            .AsNoTracking()
            .ProjectToListAsync<ProjectResponse>(_mapper.ConfigurationProvider);
        }
    }
}