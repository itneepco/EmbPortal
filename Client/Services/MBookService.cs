using Client.Extensions;
using Client.Models;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class MBookService : IMBookService
    {
        private readonly HttpClient _httpClient;
        public MBookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> CreateMeasurementBook(CreateMBookRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/MBook", request);
            return await response.ToResult<int>();
        }

        public Task<IResult<int>> CreateMeasurementBookItem(int id, MBookItemRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteMeasurementBook(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteMeasurementBookItem(int id, int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateMeasurementBook(int id, MBookRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateMeasurementBookItem(int id, int itemId, MBookItemRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
