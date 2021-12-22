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
                projectId: 5,
                contractorId: 1,
                engineerInCharge: "001234"
            );

            workOrder.AddUpdateLineItem("Line item one", 1, 100, 40);
            workOrder.AddUpdateLineItem("Line item two", 1, 50, 200);
            workOrder.AddUpdateLineItem("Line item three", 1, 500, 30);

            context.WorkOrders.Add(workOrder);
            await context.SaveChangesAsync();
        }
    }
}
