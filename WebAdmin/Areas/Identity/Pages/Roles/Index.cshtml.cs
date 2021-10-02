using System.Collections.Generic;
using System.Threading.Tasks;
using Domian.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Areas.Identity.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;
        public IndexModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IList<AppRole> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new AppRole { Name = roleName.Trim() });
            }
            return RedirectToAction("Index");
        }
    }
}
