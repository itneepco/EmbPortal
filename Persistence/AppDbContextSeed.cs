using System;
using System.Threading.Tasks;
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

            }
            catch (Exception exception)
            {
                var log = loggerFactory.CreateLogger<AppDbContextSeed>();
                log.LogError(exception.Message);
                throw;
            }
        }
    }
}