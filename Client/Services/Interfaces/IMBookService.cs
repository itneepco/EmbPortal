using Client.Models;
using EmbPortal.Shared.Requests;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IMBookService
    {
        Task<IResult> DeleteMeasurementBook(int id);
        Task<IResult<int>> CreateMeasurementBook(CreateMBookRequest request);
        Task<IResult> UpdateMeasurementBook(int id, MBookRequest request);
        Task<IResult<int>> CreateMeasurementBookItem(int id, MBookItemRequest request);
        Task<IResult> UpdateMeasurementBookItem(int id, int itemId, MBookItemRequest request);
        Task<IResult> DeleteMeasurementBookItem(int id, int itemId);
    }
}
