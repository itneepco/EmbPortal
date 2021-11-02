using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Request;

namespace Application.Projects.Command
{
   public class UpdateProjectCommand : ProjectRequest, IRequest<int>
   {
        
   }

   public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, int>
   {
      private readonly IAppDbContext _context;
      public UpdateProjectCommandHandler(IAppDbContext context)
      {
         _context = context;
      }

      public async Task<int> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
      {
         var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id);
          if(project == null) {
             return default;
         }
         project.SetName(request.Name);
         await _context.SaveChangesAsync(cancellationToken);
         return project.Id;
      }
   }
}