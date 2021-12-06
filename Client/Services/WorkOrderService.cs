using Client.Extensions;
using Client.Models;
using Client.Services.Interfaces;
using EmbPortal.Shared.Responses;
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

        public async Task<PaginatedList<WorkOrderResponse>> GetWorkOrdersByProjectPagination(int projectId, int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<WorkOrderResponse>>($"/api/WorkOrder/Project/{projectId}?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<PaginatedList<WorkOrderResponse>> GetUserWorkOrdersPagination(int pageIndex, int pageSize, string search)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<WorkOrderResponse>>($"/api/WorkOrder/Project/self?pageNumber={pageIndex}&pageSize={pageSize}&search={search}");
        }

        public async Task<IResult> DeleteWorkOrder(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/WorkOrder/{id}");
            return await response.ToResult();
        }
    }
}
