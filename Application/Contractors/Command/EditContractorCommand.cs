using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domian.Enums;
using MediatR;

namespace Application.Contractors.Command
{
   public record EditContractorCommand(int id, string name) : IRequest
    {

    }

    public class EditContractorCommandHandler : IRequestHandler<EditContractorCommand>
    {
        private readonly IAppDbContext _context;
        public EditContractorCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditContractorCommand request, CancellationToken cancellationToken)
        {
           
                var contractor = await _context.Contractors.FindAsync(request.id);
                
                if (contractor == null)
                {
                    throw new NotFoundException(nameof(contractor), request.id);
                }

                contractor.SetName(request.name);
                
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;           

        }

       
    }
}
