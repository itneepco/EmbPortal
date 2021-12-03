using Domain.Entities.WorkOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seed
{
    public class WorkOrderSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.WorkOrders.Any())
                return;

            var workOrder = new WorkOrder
            (
                orderNo: "abc123",
                orderDate: DateTime.Now,
                title: "Hello World",
                agreementNo: "bca321",
                agreementDate: DateTime.Now,
                projectId: 1,
                contractorId: 1,
                engineerInCharge: "001234"
            );

            workOrder.AddUpdateLineItem("Line item one", 10, 1, 200, 25);
            workOrder.AddUpdateLineItem("Line item two", 20, 1, 200, 25);
            workOrder.AddUpdateLineItem("Line item three", 20, 1, 200, 25);

            context.WorkOrders.Add(workOrder);
            await context.SaveChangesAsync();
        }
    }
}
