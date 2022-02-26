using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class RABillRequest
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public DateTime? BillDate { get; set; }
        public List<RABillItemRequest> Items { get; set; } = new();
    }
}
