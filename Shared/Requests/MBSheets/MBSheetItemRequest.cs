namespace EmbPortal.Shared.Requests
{
    public class MBSheetItemRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal UnitRate { get; set; }
        public string Uom { get; set; }
        public int Dimension { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }
        public int MBookItemId { get; set; }

        public float Total
        {
            get
            {
                if(Dimension == 3)
                {
                    return Value1 * Value2 * Value3;
                }
                else if (Dimension == 2)
                {
                    return Value1 * Value2;
                }
                else
                {
                    return Value1;
                }
            }
        }
    }
}
