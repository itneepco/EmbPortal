using Client.Extensions;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests.RA;
using EmbPortal.Shared.Responses;
using EmbPortal.Shared.Responses.RA;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services;

public class RAService : IRAService
{
    private readonly HttpClient _httpClient;

    public RAService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RAResponse>> GetRAsByWOrder(int wOrderId)
    {
        return await _httpClient.GetFromJsonAsync<List<RAResponse>>($"/api/RA/WOrder/{wOrderId}");
    }
    public async Task<IResult<int>> CreateRABill(RARequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/RA", request);
        return await response.ToResult<int>();
    }

    public async Task<IResult<RADetailResponse>> GetRAById(int raId)
    {
        var response = await _httpClient.GetAsync($"/api/RA/{raId}");
        return await response.ToResult<RADetailResponse>();
    }

    public async  Task<IResult> DeleteRa(int raId)
    {
        var response = await _httpClient.DeleteAsync($"/api/RA/{raId}");
        return await response.ToResult();
    }

    public async Task<IResult> EditRa(RARequest request, int raId)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/RA/{raId}", request);
        return await response.ToResult(); 
    }

    public async Task<string> GenrateRaReport(int raId)
    {
        var response = await _httpClient.GetAsync($"/api/RA/{raId}/RaPdf");
        return await response.Content.ReadAsStringAsync();        
    }

    public async Task<IResult> PublishRa(int id)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/RA/{id}/Publish","");
        return await response.ToResult();
    }

    public async Task<IResult> PostRaToSAP(int raBillId)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/RA/{raBillId}/PostToSAP", "");
        return await response.ToResult();
    }
}
