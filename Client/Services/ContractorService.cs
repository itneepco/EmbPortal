using Client.Services.Interfaces;
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

        public async Task<List<ContractorResponse>> GetAllContractors()
        {
            var result = await _httpClient.GetFromJsonAsync<List<ContractorResponse>>($"/api/Contractor/all");
            return result;
        }
    }
}
