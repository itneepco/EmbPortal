using Client.Models;
using EmbPortal.Shared.Requests;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IResult> Login(LoginRequest model);
        Task<IResult> Logout();
        Task<string> RefreshToken();
        Task<string> TryRefreshToken();
    }
}
