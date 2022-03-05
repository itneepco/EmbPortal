﻿using Client.Extensions;
using Client.Models;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System;
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

        public async Task<IResult<RABillResponse>> GetRABillById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/RABill/{id}");
            return await response.ToResult<RABillResponse>();
        }
    }
}
