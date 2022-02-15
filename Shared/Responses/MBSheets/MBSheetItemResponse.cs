namespace EmbPortal.Shared.Responses
{
    public class MBSheetItemResponse
    {
        public int Id { get; set; }
        public int Nos { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public int Dimension { get; set; }
        public decimal UnitRate { get; set; }
        public int MBookItemId { get; set; }
        public string MBookItemDescription { get; set; }
        public float TotalQuantity
        {
            get
            {
                if (Dimension == 3)
                {
                    return Nos * Value1 * Value2 * Value3;
                }
                else if (Dimension == 2)
                {
                    return Nos * Value1 * Value2;
                }
                else
                {
                    return Nos * Value1;
                }
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return (decimal)TotalQuantity * UnitRate;
            }
        }
    }
}
