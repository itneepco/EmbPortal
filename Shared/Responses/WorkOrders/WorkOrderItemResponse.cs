using EmbPortal.Shared.Enums;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderItemResponse
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public int SubItemNo { get; set; }
        public long ServiceNo { get; set; }
        public string ShortServiceDesc { get; set; }
        public string LongServiceDesc { get; set; }
        public decimal UnitRate { get; set; }
        public string Uom { get; set; }        
        public float PoQuantity { get; set; }       
    }
}
