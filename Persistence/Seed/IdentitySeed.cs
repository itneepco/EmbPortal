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
                        DisplayName = "Admin",
                        Designation = "Manager",
                        Email = "admin@neepco.com",
                        UserName = "001233",
                        PhoneNumber = "8976453420"
                    },
                    new AppUser
                    {
                        DisplayName = "Engineer InCharge",
                        Designation = "Manager",
                        Email = "eng@neepco.com",
                        UserName = "001234",
                        PhoneNumber = "8976453420"
                    },
                    new AppUser
                    {
                        DisplayName = "Validating Officer",
                        Designation = "Manager",
                        Email = "val@neepco.com",
                        UserName = "001235",
                        PhoneNumber = "9876543210"
                    },
                    new AppUser
                    {
                        DisplayName = "Measurement Officer",
                        Designation = "Manager",
                        Email = "mea@neepco.com",
                        UserName = "001236",
                        PhoneNumber = "9876543210"
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
                    await userManager.CreateAsync(user, "Pass@123");
                    await userManager.AddToRoleAsync(user, "Member");

                    if (user.Email == "admin@neepco.com")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }

                    if (user.Email == "eng@neepco.com")
                    {
                        await userManager.AddToRoleAsync(user, "Manager");
                    }

                    if (user.Email == "val@neepco.com")
                    {
                        await userManager.AddToRoleAsync(user, "Manager");
                    }
                    if (user.Email == "mea@neepco.com")
                    {
                        await userManager.AddToRoleAsync(user, "Manager");
                    }
                }
            }
        }
    }
}
