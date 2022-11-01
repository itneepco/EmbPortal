using Domain.Entities.Identity;

namespace Infrastructure.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}
