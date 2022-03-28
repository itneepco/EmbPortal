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
    public class WorkOrderService : IWorkOrderService
    {
        private readonly HttpClient _httpClient;
        public WorkOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<WorkOrderDetailResponse>> GetWorkOrderById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/WorkOrder/{id}");
            return await response.ToResult<WorkOrderDetailResponse>();
        }

        public async Task<PaginatedList<WorkOrderResponse>> GetWorkOrdersByProjectPagination(int projectId, int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<WorkOrderResponse>>($"/api/WorkOrder/Project/{projectId}?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<PaginatedList<WorkOrderResponse>> GetUserWorkOrdersPagination(int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<WorkOrderResponse>>($"/api/WorkOrder/self?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<IResult> DeleteWorkOrder(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/WorkOrder/{id}");
            return await response.ToResult();
        }

        public async Task<IResult<int>> CreateWorkOrder(WorkOrderRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/WorkOrder", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateWorkOrder(int id, WorkOrderRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/WorkOrder/{id}", request);
            return await response.ToResult();
        }

        public async Task<IResult<int>> CreateWorkOrderItem(int id, WorkOrderItemRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/WorkOrder/{id}/Item", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> UpdateWorkOrderItem(int id, int itemId, WorkOrderItemRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/WorkOrder/{id}/Item/{itemId}", request);
            return await response.ToResult();
        }

        public async Task<IResult> DeleteWorkOrderItem(int id, int itemId)
        {
            var response = await _httpClient.DeleteAsync($"/api/WorkOrder/{id}/Item/{itemId}");
            return await response.ToResult();
        }

        public async Task<IResult<List<PendingOrderItemResponse>>> GetPendingWorkOrderItems(int id)
        {
            var response = await _httpClient.GetAsync($"/api/WorkOrder/{id}/Item/Pending");
            return await response.ToResult<List<PendingOrderItemResponse>>();
        }

        public async Task<IResult> PublishWorkOrder(int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/WorkOrder/{id}/Publish", "");
            return await response.ToResult();
        }

        public async Task<IResult> PublishWorkOrderItem(int workOrderId, int orderItemId)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/WorkOrder/{workOrderId}/Items/{orderItemId}/Publish", "");
            return await response.ToResult();
        }
    }
}
