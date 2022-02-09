using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMeasurementBookService
    {
        Task<List<MBookItemApprovedQty>> GetMBItemsApprovedQty(int mBookId);
    }
}
