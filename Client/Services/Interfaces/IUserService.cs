using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<string>> GetRoles();
        Task<List<string>> GetUserRoles(string userId);
        Task<PaginatedList<UserResponse>> GetUsersPagination(int pageIndex, int pageSize, string search);
        Task<IResult> RegisterUser(UserRequest request);
        Task<IResult> UpdateUser(string userId, UserRequest request);
        Task<IResult> ResetPassword(string userId);
    }
}
