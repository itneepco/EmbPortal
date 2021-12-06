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

            var subItem1 = new SubItem("sub item1", 1, 200, 25);
            var subItem2 = new SubItem("sub item1", 1, 200, 25);
            var subItem3 = new SubItem("sub item1", 1, 200, 25);
            var subItems = new List<SubItem>
            {
                subItem1,
                subItem2,
                subItem3
            };

            workOrder.AddUpdateLineItem("Line item one", subItems);
            workOrder.AddUpdateLineItem("Line item two", subItems);
            workOrder.AddUpdateLineItem("Line item three", subItems);

            context.WorkOrders.Add(workOrder);
            await context.SaveChangesAsync();
        }
    }
}
