using EmbPortal.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EmbPortal.Shared.Responses
{
    public class MBookItemResponse
    {
        public int Id { get; set; }
        public int WorkOrderItemId { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public decimal UnitRate { get; set; }
        public float PoQuantity { get; set; }
        public int Dimension { get; set; }
        public List<MBSheetItemInfoResponse> MBSheetItems { get; set; } = new();

        public decimal TotalAmount
        {
            get
            {
                return (decimal)PoQuantity * UnitRate;
            }
        }

        public float ApprovedQuantity
        {
            get
            {
                var items = MBSheetItems.Where(p => p.Status == MBSheetStatus.ACCEPTED.ToString()).ToList();
                return items.Aggregate((float)0, (acc, curr) => acc + curr.TotalQuantity);
            }
        }
    }
}
