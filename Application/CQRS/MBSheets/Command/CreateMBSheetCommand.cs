using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MBSheets.Command
{
    public class CreateMBSheetCommand : IRequest<int>
    {
    }

    public class CreateMBSheetCommandHandler : IRequestHandler<CreateMBSheetCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateMBSheetCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<int> Handle(CreateMBSheetCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
