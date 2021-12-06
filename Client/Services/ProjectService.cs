using Client.Services.Interfaces;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ProjectService : IProjectService
    {
      private readonly HttpClient _httpClient;
        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProjectResponse>> GetAllProjects()
        {
            var result = await _httpClient.GetFromJsonAsync<List<ProjectResponse>>($"/api/Project");
            return result;
        }
    }
}