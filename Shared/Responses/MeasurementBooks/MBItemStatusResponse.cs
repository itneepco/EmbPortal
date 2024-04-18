namespace EmbPortal.Shared.Responses
{
    public class WorkOrderItemStatusResponse
    {
        public int WorkOrderItemId { get; set; }
        public int MBookItemId { get; set; }
        public string ServiceNo { get; set; } 
        public string ItemDescription { get; set; }       
        public string Uom { get; set; }
        public decimal UnitRate { get; set; }
        public float PoQuantity { get; set; }
        public float CumulativeMeasuredQty { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }

        public float AvailableRAQty
        {
            get
            {
                var available = (AcceptedMeasuredQty - TillLastRAQty).ToString("0.00");
                return float.Parse(available);
            }
        }

        public float AvailableMeasurementQty
        {
            get
            { 
                var availableMeasurementQty = (PoQuantity - CumulativeMeasuredQty).ToString("0.00");
                return float.Parse(availableMeasurementQty);
            }
        }
    }
}
