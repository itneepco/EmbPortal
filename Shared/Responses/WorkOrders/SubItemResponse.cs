namespace EmbPortal.Shared.Responses
{
    public class SubItemResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal UnitRate { get; set; }
        public string Uom { get; set; }
        public int UomId { get; set; }
        public string Dimension { get; set; }
        public float PoQuantity { get; set; }
    }
}
