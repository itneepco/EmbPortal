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
    public class MBookService : IMBookService
    {
        private readonly HttpClient _httpClient;
        public MBookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MeasurementBookResponse>> GetMBooksByWorkOrderId(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<List<MeasurementBookResponse>>($"/api/MBook/WorkOrder/{orderId}");
        }

        public async Task<PaginatedList<MBookInfoResponse>> GetMBooksByUserIdPagination(int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<MBookInfoResponse>>($"/api/MBook/CurrentUser?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<IResult<MBookDetailResponse>> GetMBooksById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/MBook/{id}");
            return await response.ToResult<MBookDetailResponse>();
        }

        public async Task<IResult> DeleteMeasurementBook(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/MBook/{id}");
            return await response.ToResult();
        }

        public async Task<IResult<int>> CreateMeasurementBook(MBookRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/MBook", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateMeasurementBook(int id, MBookRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBook/{id}", request);
            return await response.ToResult();
        }

        public async Task<IResult> PublishMeasurementBook(int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBook/{id}/Publish", "");
            return await response.ToResult();
        }
    }
}
