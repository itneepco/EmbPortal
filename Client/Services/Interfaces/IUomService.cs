using Client.Models;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IUomService
    {
        Task<List<UomResponse>> GetAllUoms();
        Task<List<UomDimensionResponse>> GetUomDimensions();
        Task<PaginatedList<UomResponse>> GetUomsPagination(int pageIndex, int pageSize, string search);
        Task<IResult<int>> CreateUom(UomRequest request);
        Task<IResult> UpdateUom(int id, UomRequest request);
        Task<IResult> DeleteUom(int id);
    }
}
