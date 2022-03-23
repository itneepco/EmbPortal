using System;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class MBSheetRequest
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public DateTime? MeasurementDate { get; set; }
        public int MeasurementBookId { get; set; }
    }
}
