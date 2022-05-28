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
    public class RABillService : IRABillService
    {
        private readonly HttpClient _httpClient;

        public RABillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> CreateRABill(RABillRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/RABill", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> DeleteRABill(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/RABill/{id}");
            return await response.ToResult();
        }

        public async Task<IResult> EditRABill(int id, RABillRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/RABill/{id}", request);
            return await response.ToResult(); ;
        }

        public async Task<List<RABillResponse>> GetRABillsByMBookId(int mBookId)
        {
            return await _httpClient.GetFromJsonAsync<List<RABillResponse>>($"/api/RABill/MBook/{mBookId}");
        }

        public async Task<List<RABillInfoResponse>> GetUserPendingRABills()
        {
            return await _httpClient.GetFromJsonAsync<List<RABillInfoResponse>>($"/api/RABill/Pending");
        }

        public async Task<IResult<RABillDetailResponse>> GetRABillById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/RABill/{id}");
            return await response.ToResult<RABillDetailResponse>();
        }

        public async Task<IResult> ApproveRABill(int raBillId)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/RABill/{raBillId}/Approve", "");
            return await response.ToResult();
        }

        public async Task<IResult> RevokeRABill(int raBillId)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/RABill/{raBillId}/Revoke", "");
            return await response.ToResult();
        }

        public async Task<string> GeneratePdf(int id)
        {
            var response = await _httpClient.GetAsync($"/api/RABill/{id}/Download");
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<int>> CreateRADeduction(int raBillId, RADeductionRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/RABill/{raBillId}/Deduction", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> DeleteRADeduction(int raBillId, int deductionId)
        {
            var response = await _httpClient.DeleteAsync($"/api/RABill/{raBillId}/Deduction/{deductionId}");
            return await response.ToResult();
        }
    }
}
