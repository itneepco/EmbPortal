using System.Collections.Generic;
using System.Threading.Tasks;
using Domian.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Areas.Identity.ViewModels;

namespace WebAdmin.Areas.Identity.Pages.UserRoles
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserRolesVM> UserRolesViewModel { get; set; } = new List<UserRolesVM>();

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            foreach (AppUser user in users)
            {
                var thisViewModel = new UserRolesVM();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                UserRolesViewModel.Add(thisViewModel);
            }
        }

        private async Task<List<string>> GetUserRoles(AppUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
