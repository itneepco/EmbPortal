using EmbPortal.Shared.Responses;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsResponse> GetDasbordStats();
    }
}
