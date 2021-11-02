using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domian;
using MediatR;
using Shared.Request;

namespace Application.Projects.Command
{
    public class CreateProjectCommand : ProjectRequest, IRequest<int>
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
             name : request.Name
         );
         _context.Projects.Add(project);
         await _context.SaveChangesAsync(cancellationToken);
         return project.Id;    
         
      }
   }
}