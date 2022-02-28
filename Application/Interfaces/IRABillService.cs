using Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRABillService
    {
        Task<List<RAItemQtyStatus>> GetRAItemQtyStatus(int mBookId);
    }
}
