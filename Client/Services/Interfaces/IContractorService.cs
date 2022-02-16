using Client.Models;
using EmbPortal.Shared.Requests;
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
        Task<PaginatedList<ContractorResponse>> GetContractorsPagination(int pageIndex, int pageSize, string search);
        Task<IResult<int>> CreateContractor(ContractorRequest request);
        Task<IResult> UpdateContractor(int id, ContractorRequest request);
        Task<IResult> DeleteContractor(int id);
    }
}
