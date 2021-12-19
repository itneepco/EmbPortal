using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using EmbPortal.Shared.Requests;
using MediatR;

namespace Application.Uoms.Command
{
    public record CreateUomCommand(UomRequest Data) : IRequest<int>
    {
    }

    public class CreateUomCommandHandler : IRequestHandler<CreateUomCommand, int>
    {
        private readonly IAppDbContext _context;
        public CreateUomCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateUomCommand request, CancellationToken cancellationToken)
        {
            var uom = new Uom
            (
                name: request.Data.Name,
                description: request.Data.Description,
                dimension: (UomDimension) request.Data.Dimension
            );

            _context.Uoms.Add(uom);
            await _context.SaveChangesAsync(cancellationToken);
            return uom.Id;
        }
    }
}