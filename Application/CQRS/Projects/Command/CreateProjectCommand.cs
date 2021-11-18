using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Command
{
    public record CreateProjectCommand(string name) : IRequest<int>
    {   
    }

   public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
   {
      private readonly IAppDbContext _context;
       public CreateProjectCommandHandler(IAppDbContext context)
       {
         _context = context;
           
       }

      public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
      {
         var project = new Project 
         (
             name : request.name
         );
         _context.Projects.Add(project);
         await _context.SaveChangesAsync(cancellationToken);
         return project.Id;    
         
      }
   }
}