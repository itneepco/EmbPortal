using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contractors.Command
{
    public record DeleteContractorCommand(int id) : IRequest
    {   
    }

    public class DeleteContractorCommandHandler : IRequestHandler<DeleteContractorCommand>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteContractorCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteContractorCommand request, CancellationToken cancellationToken)
        {
            var contractor = await _dbContext.Contractors
                .Include(p => p.WorkOrders)
                .FirstOrDefaultAsync(x => x.Id == request.id);

            if (contractor == null)
            {
                throw new NotFoundException(nameof(contractor), request.id);
            }

            // Check if this contractor has any work order associated with it
            bool hasWorkOrders = contractor.WorkOrders.Count > 0 ? true : false;
            if (hasWorkOrders)
            {
                throw new DeleteFailureException(nameof(contractor), request.id, 
                    "This entity is being referenced by Work Order.");
            }

            _dbContext.Contractors.Remove(contractor);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}