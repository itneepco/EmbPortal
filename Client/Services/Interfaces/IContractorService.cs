using EmbPortal.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IContractorService
    {
        Task<List<ContractorResponse>> GetAllContractors();
    }
}
