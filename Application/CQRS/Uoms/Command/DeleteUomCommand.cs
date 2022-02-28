using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Uoms.Command
{
   public record DeleteUomCommand(int Id) : IRequest
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
         var uom = await _context.Uoms
                .Include(p => p.WorkOrderItems)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

         if (uom == null)
         {
            throw new NotFoundException(nameof(uom), request.Id);
         }

         // Check if work order items are associated with this uom
         bool hasWorkOrders = uom.WorkOrderItems.Count > 0 ? true : false;
         if (hasWorkOrders)
         {
            throw new DeleteFailureException(nameof(uom), request.Id, 
                  "This entity is being referenced by Work Order Item.");
         }

         _context.Uoms.Remove(uom);
         await _context.SaveChangesAsync(cancellationToken);
         return Unit.Value;
      }
   }
}