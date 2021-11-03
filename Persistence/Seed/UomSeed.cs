using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Persistence.Seed
{
    public class UomSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Uoms.Any())
                return;
            var Uoms = new List<Uom>()
            {
                new Uom("Cum", UomDimension.THREEDIMENSION),
                new Uom("Sqm", UomDimension.TWODIMENSION),
                new Uom("Rum", UomDimension.ONEDIMENSION),
                new Uom("Kg",  UomDimension.ONEDIMENSION)
            };

            context.Uoms.AddRange(Uoms);
            await context.SaveChangesAsync();
        }
    }
}