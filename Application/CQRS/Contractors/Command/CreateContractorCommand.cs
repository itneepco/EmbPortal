using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Shared.Requests;

namespace Application.Contractors.Command
{
    public record CreateContractorCommand(string name) : IRequest<int>
    {
        public static implicit operator CreateContractorCommand(ContractorRequest v)
        {
            throw new NotImplementedException();
        }
    }

    public class CraeteContractorCommandHandler : IRequestHandler<CreateContractorCommand, int>
    {
        private readonly IAppDbContext _context;
        public CraeteContractorCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateContractorCommand request, CancellationToken cancellationToken)
        {
            var Contractor = new Contractor
            (
              name: request.name
            );

            _context.Contractors.Add(Contractor);
            await _context.SaveChangesAsync(cancellationToken);
            return Contractor.Id;
        }
    }
}