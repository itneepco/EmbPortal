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

        public async Task<IResult<int>> CreateProject(ProjectRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Project", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateProject(int id, ProjectRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Project/{id}", request);
            return await response.ToResult();
        }

        public async Task<IResult> DeleteProject(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Project/{id}");
            return await response.ToResult();
        }
    }
}