using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class MBSheetItemRequest
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Nos should be atleast 1")]
        public int Nos { get; set; } = 1;
        
        [Required] 
        public string Uom { get; set; }
        
        [Range(1, 3)] 
        public int Dimension { get; set; }
        
        [Required, MaxLength(100)] 
        public string Description { get; set; }

        public string MBookItemDescription { get; set; }

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
    }
}
