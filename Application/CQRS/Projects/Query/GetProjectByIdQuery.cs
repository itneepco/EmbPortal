using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Responses;

namespace Application.Projects.Query
{
    public record GetProjectByIdQuery(int id) : IRequest<ProjectResponse>
    {
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
            var project = await _context.Projects
                .ProjectTo<ProjectResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.id);

            if (project == null)
            {
                throw new NotFoundException(nameof(project), request.id);
            }

            return project;
        }
    }
}