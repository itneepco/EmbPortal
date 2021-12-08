using System.Collections.Generic;
using System.Linq;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderItemResponse
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<SubItemResponse> SubItems { get; set; }

        public float Quantity 
        {
            get
            {
                return SubItems.Aggregate(0, (float acc, SubItemResponse item) => acc + item.PoQuantity);
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return SubItems.Aggregate(0, (decimal acc, SubItemResponse item) => acc + (item.UnitRate*(decimal)item.PoQuantity));
            }
        }
    }
}
