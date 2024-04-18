using EmbPortal.Shared.Requests.RA;
using EmbPortal.Shared.Requests;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace EmbPortal.Shared.Responses.RA;

public class RaDto
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
    public int WorkOrderId { get; set; }
    public List<RaItemDto> Items { get; set; } = new();
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
public class RaItemDto
{
    public int WorkOrderItemId { get; set; }
    public string ItemDescription { get; set; }
    public int ItemNo { get; set; }
    public int SubItemNo { get; set; }
    public long ServiceNo { get; set; }
    public string ShortServiceDesc { get; set; }
    public string Uom { get; set; }
    public decimal UnitRate { get; set; }
    public float PoQuantity { get; set; }
    public float MeasuredQty { get; set; }
    public float TillLastRAQty { get; set; }

    [Range(0, float.MaxValue)]
    public float CurrentRAQty { get; set; }

    [MaxLength(100)]
    public string Remarks { get; set; }

    public float AvailableQty => MeasuredQty - TillLastRAQty;

    public decimal AvailableAmount => UnitRate * (decimal)AvailableQty;

    public decimal CurrentRAAmount => UnitRate * (decimal)CurrentRAQty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CurrentRAQty > AvailableQty)
        {
            yield return new ValidationResult(GetErrorMessage(), new[] { nameof(CurrentRAQty) });
        }
    }
    private string GetErrorMessage()
    {
        return $"Current RA Qty {CurrentRAQty} must be less than or equal to {AvailableQty.ToString("0.00")}";
    }
}
public class RaDeductionDto
{
    [Required, MaxLength(255)]
    public string Description { get; set; }
    public decimal Amount { get; set; }
}