using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class MBSheetItemResponse
    {
        public int Id { get; set; }
        public long ServiceNo { get; set; }
        public string ShortServiceDesc { get; set; }
        public int Nos { get; set; }
        public float MeasuredQuantity { get; set; }
        
        public string Description { get; set; }
        public string Uom { get; set; }
        public int Dimension { get; set; }
        public decimal UnitRate { get; set; }
        public int MBSheetId { get; set; }
        public int MBookItemId { get; set; }
        public List<ItemAttachmentResponse> Attachments { get; set; }

        public decimal TotalAmount
        {
            get
            {
                return (decimal)MeasuredQuantity * UnitRate;
            }
        }
    }
}
