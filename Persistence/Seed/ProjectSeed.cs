using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Persistence.Seed
{
    public class ProjectSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Projects.Any())
                return;
            var Projects = new List<Project>()
            {
                new Project("GUWAHATI"),
                new Project("SHILLONG"),
                new Project("NEW DELHI"),
                new Project("KOLKATA"),
                new Project("AGBPP"),
                new Project("KHEP"),
                new Project("RHEP"),
                new Project("DHEP"),
                new Project("KaHEP"),
                new Project("AGTCCPP"),
                new Project("TGBP"),
                new Project("PHEP"),
                new Project("TrHEP"),
            };

            context.Projects.AddRange(Projects);
            await context.SaveChangesAsync();
        }
    }
}