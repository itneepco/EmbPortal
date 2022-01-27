using Client.Extensions;
using Client.Models;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class MBSheetService : IMBSheetService
    {
        private readonly HttpClient _httpClient;

        public MBSheetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> CreateMBSheet(MBSheetRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/MBSheet", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> DeleteMBSheet(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/MBSheet/{id}");
            return await response.ToResult();
        }

        public async Task<List<MBSheetResponse>> GetMBSheetsByMBookId(int mBookId)
        {
            return await _httpClient.GetFromJsonAsync<List<MBSheetResponse>>($"/api/MBSheet/MBook/{mBookId}");
        }
        public async Task<IResult> AcceptMBSheet(int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{id}/Accept", "");
            return await response.ToResult();
        }

        public async Task<IResult> ValidateMBSheet(int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{id}/Validate", "");
            return await response.ToResult();
        }
    }
}
