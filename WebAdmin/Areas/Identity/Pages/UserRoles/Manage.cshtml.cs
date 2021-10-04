using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domian.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdmin.Areas.Identity.ViewModels;

namespace WebAdmin.Areas.Identity.Pages.UserRoles
{
    public class ManageModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public ManageModel(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public List<ManageUserRolesVM> UserRoles { get; set; } = new List<ManageUserRolesVM>();

        [BindProperty]
        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            UserId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with Id = {userId} cannot be found");
            }

            ViewData["userName"] = user.UserName;
            
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                UserRoles.Add(userRolesViewModel);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Page();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }
            result = await _userManager.AddToRolesAsync(user, UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
