using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MBSheetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public record DeleteMBSheetItemCommand(int Id, int MBSheetId) : IRequest
    {
    }

    public class DeleteMBSheetItemCommandHandler : IRequestHandler<DeleteMBSheetItemCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteMBSheetItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteMBSheetItemCommand request, CancellationToken cancellationToken)
        {
            MBSheet mbSheet = await _context.MBSheets
                .Include(p => p.Items)
                .Where(p => p.Id == request.MBSheetId)
                .FirstOrDefaultAsync();

            if (mbSheet == null)
            {
                throw new NotFoundException(nameof(MBSheet), request.MBSheetId);
            }

            var mbSheetItem = mbSheet.Items.FirstOrDefault(p => p.Id == request.Id);

            if (mbSheet == null)
            {
                throw new NotFoundException($"Current MB Sheet does not have line item with Id: {request.Id}");
            }

            mbSheet.RemoveLineItem(mbSheetItem);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
