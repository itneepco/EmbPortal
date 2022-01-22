using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public class EditMBSheetCommand : IRequest
    {
    }

    public class EditMBSheetCommandHandler : IRequestHandler<EditMBSheetCommand>
    {
        private readonly IAppDbContext _context;

        public EditMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(EditMBSheetCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
