using System;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.Seed;

namespace Persistence
{
    public class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context, ILoggerFactory loggerFactory)
        {
            
        }
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            await IdentitySeed.SeedAsync(userManager, roleManager);
        }
    }
}