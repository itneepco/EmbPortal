using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace EmbPortal.Shared.Requests.RA;

public class RARequest
{
    [Required]
    public DateTime? BillDate { get; set; }
    [Required]
    public DateTime? FromDate { get; set; }
    [Required]
    public DateTime? ToDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    [Required, MaxLength(250)]
    public string Remarks { get; set; } = string.Empty;
    [Required, MaxLength(250)]
    public string LastBillDetail { get; set; } = string.Empty;
    public int WorkOrderId { get; set; }
    public List<RAItemRequest> Items { get; set; } = new();
    public List<DeductionRequest> Deductions { get; set; } = new();
    
    public decimal GetTotalRAAmount()
    {
        return Items.Aggregate((decimal)0, (curr, item) => curr + item.CurrentRAAmount);
    }
    public decimal GetTotalDeduction()
    {
        return Deductions.Aggregate((decimal)0, (curr, item) => curr + item.Amount);
    }
    public decimal GetNetRAAmount()
    {
      return GetTotalRAAmount() - GetTotalDeduction();
    }
}
