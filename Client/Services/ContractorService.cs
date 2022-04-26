using Client.Extensions;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ContractorService : IContractorService
    {
        private readonly HttpClient _httpClient;
        public ContractorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaginatedList<ContractorResponse>> GetContractorsPagination(int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<ContractorResponse>>($"/api/Contractor?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<List<ContractorResponse>> GetAllContractors()
        {
            var result = await _httpClient.GetFromJsonAsync<List<ContractorResponse>>($"/api/Contractor/all");
            return result;
        }

        public async Task<IResult<int>> CreateContractor(ContractorRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Contractor", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateContractor(int id, ContractorRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Contractor/{id}", request);
            return await response.ToResult();
        }

        public async Task<IResult> DeleteContractor(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Contractor/{id}");
            return await response.ToResult();
        }
    }
}
