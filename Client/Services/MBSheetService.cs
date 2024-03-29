﻿using Client.Extensions;
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

        public async Task<IResult> EditMBSheet(int mbSheetId, MBSheetRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{mbSheetId}", request);
            return await response.ToResult();
        }

        public async Task<IResult> PublishMBSheet(int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{id}/Publish", "");
            return await response.ToResult();
        }

        public async Task<IResult> DeleteMBSheet(int mbSheetId)
        {
            var response = await _httpClient.DeleteAsync($"/api/MBSheet/{mbSheetId}");
            return await response.ToResult();
        }

        public async Task<List<MBSheetInfoResponse>> GetMBSheetsByMBookId(int mBookId)
        {
            return await _httpClient.GetFromJsonAsync<List<MBSheetInfoResponse>>($"/api/MBSheet/MBook/{mBookId}");
        }

        public async Task<IResult<MBSheetResponse>> GetMBSheetById(int mbSheetId)
        {
            var response = await _httpClient.GetAsync($"/api/MBSheet/{mbSheetId}");
            return await response.ToResult<MBSheetResponse>();
        }

        public async Task<IResult> AcceptMBSheet(int mbSheetId)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{mbSheetId}/Accept", "");
            return await response.ToResult();
        }

        public async Task<IResult> ValidateMBSheet(int mbSheetId)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{mbSheetId}/Validate", "");
            return await response.ToResult();
        }

        public async Task<IResult<int>> CreateMBSheetItem(int mbSheetId, MBSheetItemRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/MBSheet/{mbSheetId}/Item", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateMBSheetItem(int mbSheetId, int itemId, MBSheetItemRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/MBSheet/{mbSheetId}/Item/{itemId}", request);
            return await response.ToResult();
        }

        public async Task<IResult> DeleteMBSheetItem(int mbSheetId, int itemId)
        {
            var response = await _httpClient.DeleteAsync($"/api/MBSheet/{mbSheetId}/Item/{itemId}");
            return await response.ToResult();
        }

        public async Task<IResult<List<UploadResult>>> UploadFiles(int mbSheetId, int itemId, MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync($"/api/MBSheet/{mbSheetId}/Item/{itemId}/Attachment", content);
            return await response.ToResult<List<UploadResult>>();
        }

        public async Task<IResult> DeleteMBSheetItemAttachment(int mbSheetId, int itemId, int attachmentId)
        {
            var response = await _httpClient.DeleteAsync($"/api/MBSheet/{mbSheetId}/Item/{itemId}/Attachment/{attachmentId}");
            return await response.ToResult();
        }

        public async Task<string> DownloadMBSheetItemAttachment(int mbSheetId, int itemId, int attachmentId)
        {
            var response = await _httpClient.GetAsync($"/api/MBSheet/{mbSheetId}/Item/{itemId}/Attachment/{attachmentId}/Download");
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<List<MBSheetInfoResponse>> GetPendingValidationMBSheets()
        {
            return await _httpClient.GetFromJsonAsync<List<MBSheetInfoResponse>>($"/api/MBSheet/Pending/Validation");
        }

        public async Task<List<MBSheetInfoResponse>> GetPendingApprovalMBSheets()
        {
            return await _httpClient.GetFromJsonAsync<List<MBSheetInfoResponse>>($"/api/MBSheet/Pending/Approval");
        }

        public async Task<IResult<MBSheetItemResponse>> GetMBSheetItemById(int mbSheetId, int mbSheetItemId)
        {
            var response = await _httpClient.GetAsync($"/api/MBSheet/{mbSheetId}/Items/{mbSheetItemId}");
            return await response.ToResult<MBSheetItemResponse>();
        }
    }
}
