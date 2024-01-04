using System;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests.RABill;

public class RABillHeaderRequest
{
    [Required]
    public DateTime? FromDate { get; set; }
    [Required]
    public DateTime? ToDate { get; set; }
    public  DateTime? CompletionDate { get; set; }
    public string Remarks { get; set; } = string.Empty;
    [Required]
    public string LastBillDetail { get; set; } = string.Empty;
}
