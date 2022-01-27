namespace EmbPortal.Shared.Responses
{
    public class MBSheetItemInfoResponse
    {
        public int Id { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }
        public int Dimension { get; set; }
        public string Status { get; set; }

        public float TotalQuantity
        {
            get
            {
                if (Dimension == 3)
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
