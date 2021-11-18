using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Command
{
    public record EditProjectCommand(int id, string name) : IRequest
    {
    }

    public class UpdateProjectCommandHandler : IRequestHandler<EditProjectCommand>
    {
        private readonly IAppDbContext _context;
        public UpdateProjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.id);

            if (project == null)
            {
                throw new NotFoundException(nameof(project), request.id);
            }

            project.SetName(request.name);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}