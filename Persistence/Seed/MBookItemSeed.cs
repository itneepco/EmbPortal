using Domain.Entities.MeasurementBookAggregate;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Seed
{
    public class MBookItemSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.MeasurementBooks.Any())
                return;

            //var mBook = new MeasurementBook
            //(
            //    workOrderId: 1,
            //    title: "Hello World",
            //    measurementOfficer: "001234",
            //    validatingOfficer: "002345"
            //);

            //mBook.AddUpdateLineItem(1);
            //mBook.AddUpdateLineItem(2);

            //context.MeasurementBooks.Add(mBook);
            //await context.SaveChangesAsync();
        }
    }
}
