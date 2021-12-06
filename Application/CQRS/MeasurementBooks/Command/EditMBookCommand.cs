using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using EmbPortal.Shared.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Command
{
    public record EditMBookCommand(int id, MBookRequest data) : IRequest
    {
    }

    public class EditMBCommandHandler : IRequestHandler<EditMBookCommand>
    {
        private readonly IAppDbContext _context;

        public EditMBCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditMBookCommand req, CancellationToken cancellationToken)
        {
            var measurementBook = await _context.MeasurementBooks.FindAsync(req.id);

            if (measurementBook == null)
            {
                throw new NotFoundException(nameof(measurementBook), req.id);
            }

            measurementBook.SetWorkOrderId(req.data.WorkOrderId);
            measurementBook.SetTitle(req.data.Title);
            measurementBook.SetMeasurementOfficer(req.data.MeasurementOfficer);
            measurementBook.SetValidatingOfficer(req.data.ValidatingOfficer);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
