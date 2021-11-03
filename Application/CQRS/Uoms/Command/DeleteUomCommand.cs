using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Uoms.Command
{
   public record DeleteUomCommand(int id) : IRequest
   {

   }

   public class DeleteUomCommandHandler : IRequestHandler<DeleteUomCommand>
   {
      private readonly IAppDbContext _context;
      public DeleteUomCommandHandler(IAppDbContext context)
      {
         _context = context;

      }

      public async Task<Unit> Handle(DeleteUomCommand request, CancellationToken cancellationToken)
      {
         var uom = await _context.Uoms.FindAsync(request.id);
         if (uom == null)
         {
            throw new NotFoundException(nameof(uom), request.id);
         }
         _context.Uoms.Remove(uom);
         await _context.SaveChangesAsync(cancellationToken);
         return Unit.Value;
      }
   }
}