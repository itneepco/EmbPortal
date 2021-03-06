using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using EmbPortal.Shared.Extensions;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Email => _httpContextAccessor.HttpContext?.User?.GetEmailFromClaimsPrincipal();

        public string EmployeeCode => _httpContextAccessor.HttpContext?.User?.GetEmployeeCodeFromClaimsPrincipal();

        public string DisplayName => _httpContextAccessor.HttpContext?.User?.GetUserNameFromClaimsPrincipal();
    }
}
