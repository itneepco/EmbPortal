using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Command
{
    public record DeleteProjectCommand(int id) : IRequest
    {
    }

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteProjectCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
                .Include(p => p.WorkOrders)
                .FirstOrDefaultAsync(x => x.Id == request.id);

            if (project == null)
            {
                throw new NotFoundException(nameof(project), request.id);
            }

            // Check if work orders are associated with this project
            bool hasWorkOrders = project.WorkOrders.Count > 0 ? true : false;
            if (hasWorkOrders)
            {
                throw new DeleteFailureException(nameof(project), request.id, 
                    "This entity is being referenced by Work Order.");
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}