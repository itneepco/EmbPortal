using Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMeasurementBookService
    {
        Task<List<MBookItemQtyStatus>> GetMBItemsQtyStatus(int mBookId);
    }
}
