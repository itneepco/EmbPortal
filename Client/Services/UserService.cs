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
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserResponse>>($"/api/Identity/all");
        }

        public async Task<List<string>> GetRoles()
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"/api/Identity/Roles");
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"/api/Identity/{userId}/roles");
        }

        public async Task<PaginatedList<UserResponse>> GetUsersPagination(int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<UserResponse>>($"/api/Identity?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<IResult> RegisterUser(UserRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Identity/Register", request);
            return await response.ToResult();
        }

        public async Task<IResult> ResetPassword(string userId)
        {
            var response = await _httpClient.GetAsync($"/api/Identity/{userId}/Reset");
            return await response.ToResult();
        }

        public async Task<IResult> UpdateUser(string userId, UserRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Identity/{userId}", request);
            return await response.ToResult();
        }
    }
}
