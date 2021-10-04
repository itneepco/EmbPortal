using Domian.Enums;
using Domian.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager,
           RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser> {
                    new AppUser
                    {
                        FirstName = "Bob",
                        LastName = "John",
                        Email = "bob@test.com",
                        UserName = "bob@test.com"
                    },
                    new AppUser
                    {
                        FirstName = "Admin",
                        LastName = "",
                        Email = "admin@test.com",
                        UserName = "admin@test.com"
                    }
                };

                var roles = new List<AppRole>
                {
                    new AppRole { Name = Role.ADMIN.ToString() },
                    new AppRole { Name = Role.MANAGER.ToString() },
                    new AppRole { Name = Role.COORDINATOR.ToString() },
                    new AppRole { Name = Role.MEMBER.ToString() }
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, Role.MEMBER.ToString());

                    if (user.Email == "admin@test.com")
                    {
                        await userManager.AddToRoleAsync(user, Role.ADMIN.ToString());
                    }
                }
            }
        }
    }
}
