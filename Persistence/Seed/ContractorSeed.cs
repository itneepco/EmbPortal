using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Seed
{
    public class ContractorSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Contractors.Any())
                return;

            var contractors = new List<Contractor>()
            {
                new Contractor("BB Graphics"),
                new Contractor("Amazona"),
                new Contractor("AT Infographics"),
            };

            context.Contractors.AddRange(contractors);
            await context.SaveChangesAsync();
        }
    }
}
