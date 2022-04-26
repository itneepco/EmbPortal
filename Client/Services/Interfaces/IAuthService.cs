using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
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
