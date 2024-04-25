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
        public decimal PoQuantity { get; set; }
        public decimal CumulativeMeasuredQty { get; set; }
        public decimal AcceptedMeasuredQty { get; set; }      
        
        public decimal AvailableMeasurementQty
        {
            get
            {
                return PoQuantity - CumulativeMeasuredQty;
               
            }
        }
    }
}
