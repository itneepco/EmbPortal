using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkOrderItemService
    {
        Task<bool> IsBalanceQtyAvailable(int wOrderItemId, float quantity);
    }
}
