using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using EmbPortal.Shared.Enums;
using MediatR;

namespace Application.Uoms.Command
{
    public record EditUomCommand(int id, string name, int dimension) : IRequest
    {
    }

    public class EditUomCommandHandler : IRequestHandler<EditUomCommand>
    {
        private readonly IAppDbContext _context;
        public EditUomCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditUomCommand request, CancellationToken cancellationToken)
        {

            var uom = await _context.Uoms.FindAsync(request.id);

            if (uom == null)
            {
                throw new NotFoundException(nameof(uom), request.id);
            }

            uom.Name = request.name;
            uom.Dimension = (UomDimension)request.dimension;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}