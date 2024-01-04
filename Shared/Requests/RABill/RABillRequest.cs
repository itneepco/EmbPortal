using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests;
public class RABillRequest
{        
    [Required]
    public DateTime? BillDate { get; set; }
    [Required]
    public DateTime? FromDate { get; set; }
    [Required]
    public DateTime? ToDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string Remarks { get; set; } = string.Empty;
    [Required]
    public string LastBillDetail { get; set; } = string.Empty;
    public int MeasurementBookId { get; set; }
    public List<RABillItemRequest> Items { get; set; } = new();
}
