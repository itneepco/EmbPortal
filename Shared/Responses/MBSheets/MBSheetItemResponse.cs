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
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public int Dimension { get; set; }
        public decimal UnitRate { get; set; }
        public int MBSheetId { get; set; }
        public int MBookItemId { get; set; }
        public List<ItemAttachmentResponse> Attachments { get; set; }

        public float Quantity
        {
            get
            {
                float val;

                if (Dimension == 3)
                {
                    val = Nos * Value1 * Value2 * Value3;
                }
                else if (Dimension == 2)
                {
                    val = Nos * Value1 * Value2;
                }
                else
                {
                    val = Nos * Value1;
                }

                return (float)Math.Round(val * 100f) / 100f;
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return (decimal)Quantity * UnitRate;
            }
        }
    }
}
