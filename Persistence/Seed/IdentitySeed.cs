using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager,
          RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser> {
                    new AppUser
                    {
                        DisplayName = "Bob Willis",
                        Designation = "Manager",
                        Email = "bob@test.com",
                        UserName = "001234",
                        PhoneNumber = "9876543210"
                    },
                    new AppUser
                    {
                        DisplayName = "Admin",
                        Designation = "Manager",
                        Email = "admin@neepco.com",
                        UserName = "001235",
                        PhoneNumber = "8976453420"
                    }
                };

                var roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "Admin"},
                    new IdentityRole { Name = "Manager" },
                    new IdentityRole { Name = "Member" }
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Member");

                    if (user.Email == "admin@test.com")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
    }
}
