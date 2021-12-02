using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Shared.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record EditMBCommand(int id, MBookRequest data) : IRequest
    {
    }

    public class EditMBCommandHandler : IRequestHandler<EditMBCommand>
    {
        private readonly IAppDbContext _context;

        public EditMBCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditMBCommand request, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks.FindAsync(request.id);

            if (measurementBook == null)
            {
                throw new NotFoundException(nameof(measurementBook), request.id);
            }

            measurementBook.SetWorkOrderId(request.data.WorkOrderId);
            measurementBook.SetMeasurementOfficer(request.data.MeasurementOfficer);
            measurementBook.SetValidatingOfficer(request.data.ValidatingOfficer);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
