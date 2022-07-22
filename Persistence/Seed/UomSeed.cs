using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using EmbPortal.Shared.Enums;

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
                new Uom("Cum", "Cubic Meter", UomDimension.THREEDIMENSION),
                new Uom("M", "Square Meter", UomDimension.ONEDIMENSION),
                new Uom("M2", "Square Meter", UomDimension.TWODIMENSION),
                new Uom("MN", "MN", UomDimension.ONEDIMENSION),
                new Uom("QT", "QT", UomDimension.ONEDIMENSION),
                new Uom("Kg",  "Kilogram", UomDimension.ONEDIMENSION)
            };

            context.Uoms.AddRange(Uoms);
            await context.SaveChangesAsync();
        }
    }
}