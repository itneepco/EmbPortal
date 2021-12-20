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
    public class UomService : IUomService
    {
        private readonly HttpClient _httpClient;
        public UomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<UomResponse>> GetAllUoms()
        {
            return await _httpClient.GetFromJsonAsync<List<UomResponse>>($"/api/Uom/All");
        }

        public async Task<List<UomDimensionResponse>> GetUomDimensions()
        {
            return await _httpClient.GetFromJsonAsync<List<UomDimensionResponse>>($"/api/Uom/Dimension");
        }

        public async Task<PaginatedList<UomResponse>> GetUomsPagination(int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<UomResponse>>($"/api/Uom?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<IResult<int>> CreateUom(UomRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Uom", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateUom(int id, UomRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Uom/{id}", request);
            return await response.ToResult();
        }

        public async Task<IResult> DeleteUom(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Uom/{id}");
            return await response.ToResult();
        }
    }
}
