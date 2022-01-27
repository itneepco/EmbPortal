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
            try
            {
                await ProjectSeed.SeedAsync(context);                
                await UomSeed.SeedAsync(context);
                await ContractorSeed.SeedAsync(context);
                await WorkOrderSeed.SeedAsync(context);
                await MBookSeed.SeedAsync(context);
            }
            catch (Exception exception)
            {
                var log = loggerFactory.CreateLogger<AppDbContextSeed>();
                log.LogError(exception.Message);
                throw;
            }
        }
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            await IdentitySeed.SeedAsync(userManager, roleManager);
        }
    }
}