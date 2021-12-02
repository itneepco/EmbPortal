using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Command
{
    public record CreateMeasurementBookCommand(string name) : IRequest<int>
    {
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateMeasurementBookCommand, int>
    {
        private readonly IAppDbContext _context;
        public CreateProjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateMeasurementBookCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(name: request.name);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            return project.Id;
        }
    }
}