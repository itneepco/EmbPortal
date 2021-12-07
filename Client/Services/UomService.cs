using Client.Services.Interfaces;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class UomService : IUomService
    {
        private readonly HttpClient _httpClient;
        public UomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UomResponse>> GetAllUoms()
        {
            var result = await _httpClient.GetFromJsonAsync<List<UomResponse>>($"/api/Uom");
            return result;
        }
    }
}
