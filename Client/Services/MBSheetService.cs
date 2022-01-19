using Client.Extensions;
using Client.Models;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<MBSheetResponse>> GetMBSheetsByMBookId(int mBookId)
        {
            return await _httpClient.GetFromJsonAsync<List<MBSheetResponse>>($"/api/MBSheet/MBook/{mBookId}");
        }
    }
}
