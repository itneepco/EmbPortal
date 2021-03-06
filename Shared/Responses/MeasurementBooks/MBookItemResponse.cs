using System;

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
        public float AcceptedMeasuredQty { get; set; }
        public float CumulativeMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }

        public decimal TotalAmount
        {
            get
            {
                return (decimal)PoQuantity * UnitRate;
            }
        }

        public int MeasurementProgess
        {
            get
            {
                return (int)Math.Round(CumulativeMeasuredQty / PoQuantity*100);
            }
        }

        public float BalanceQuantity
        {
            get
            {
                return PoQuantity - CumulativeMeasuredQty;
            }
        }
    }
}
