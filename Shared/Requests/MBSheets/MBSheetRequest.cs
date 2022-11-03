using System;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class MBSheetRequest
    {
        
        [Required]
        public DateTime? MeasurementDate { get; set; }
        public int MeasurementBookId { get; set; }
    }
}
