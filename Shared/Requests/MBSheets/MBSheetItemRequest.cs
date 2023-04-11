using System;
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
             
        [Required, MaxLength(100)] 
        public string Description { get; set; }

        public string MBookItemDescription { get; set; }
        public float MeasuredQuantity { get; set; }
        public int MBookItemId { get; set; }       
    }
}
