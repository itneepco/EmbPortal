using EmbPortal.Shared.Responses.MBSheets;
using System.Collections.Generic;
using System.Linq;

namespace EmbPortal.Shared.Responses
{
    public class MBSheetItemResponse
    {
        public int Id { get; set; }
        public int MBSheetId { get; set; }
        public int WorkOrderItemId { get; set; }
        public int MBookItemId { get; set; }

        public long ServiceNo { get; set; }
        public string ShortServiceDesc { get; set; }
        public string Uom { get; set; }
        public decimal UnitRate { get; set; }

        public List<ItemAttachmentResponse> Attachments { get; set; }
        public List<MBSheetItemMeasurementResponse> Measurements { get; set; }

        public float MeasuredQuantity { 
            get
            {
                return Measurements.Sum(p => p.Total);
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return (decimal)MeasuredQuantity * UnitRate;
            }
        }
    }
}
