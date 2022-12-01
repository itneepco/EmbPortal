using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class RABillRequest
    {        
        [Required]
        public DateTime? BillDate { get; set; }
        public int MeasurementBookId { get; set; }
        public List<RABillItemRequest> Items { get; set; } = new();
    }
}
