using Client.Services.Interfaces;
using EmbPortal.Shared.Responses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardStatsResponse> GetDasbordStats()
        {
            return await _httpClient.GetFromJsonAsync<DashboardStatsResponse>($"/api/Dashboard/Stats");
        }
    }
}
