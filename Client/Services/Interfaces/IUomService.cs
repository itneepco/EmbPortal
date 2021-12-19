using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IUomService
    {
        Task<List<UomResponse>> GetAllUoms();
        Task<PaginatedList<UomResponse>> GetUomsPagination(int pageIndex, int pageSize, string search);
    }
}
