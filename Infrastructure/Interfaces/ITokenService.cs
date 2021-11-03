using Domain.Entities.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
