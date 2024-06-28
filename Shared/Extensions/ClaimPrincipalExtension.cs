using System.Linq;
using System.Security.Claims;

namespace EmbPortal.Shared.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetEmailFromClaimsPrincipal(this ClaimsPrincipal user)
    {
        return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    }

    public static string GetUserNameFromClaimsPrincipal(this ClaimsPrincipal user)
    {
        return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
    }

    public static string GetEmployeeCodeFromClaimsPrincipal(this ClaimsPrincipal user)
    {
        return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
