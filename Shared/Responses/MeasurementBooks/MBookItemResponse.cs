namespace EmbPortal.Shared.Responses
{
    public class MBookItemResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ItemNo { get; set; }
        public decimal UnitRate { get; set; }
        public UomResponse Uom { get; set; }
        public float PoQuantity { get; set; }
    }
}
