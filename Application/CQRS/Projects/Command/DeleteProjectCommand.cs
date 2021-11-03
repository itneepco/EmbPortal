using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Command
{
    public record DeleteProjectCommand(int id) : IRequest<int>
    {
        
    }

   public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, int>
   {
      private readonly IAppDbContext _dbContext;
      public DeleteProjectCommandHandler(IAppDbContext dbContext)
      {
         _dbContext = dbContext;
      }

      public async Task<int> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
      {
         var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == request.id);
         if(project == null) {
             return default;
         }
         _dbContext.Projects.Remove(project);
         await _dbContext.SaveChangesAsync(cancellationToken);
         return project.Id;
      }
   }
}