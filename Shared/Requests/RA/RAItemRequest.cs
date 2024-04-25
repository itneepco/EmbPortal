using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests.RA;

public class RAItemRequest
{
    public int WorkOrderItemId { get; set; }
    public string ItemDescription { get; set; }
    public int ItemNo { get; set; }
    public string PackageNo { get; set; }
    public int SubItemNo { get; set; }
    public string SubItemPackageNo { get; set; }
    public long ServiceNo { get; set; }
    public string ShortServiceDesc { get; set; }
    public string Uom { get; set; }
    public decimal UnitRate { get; set; }
    public decimal PoQuantity { get; set; }
    public decimal MeasuredQty { get; set; }
    public decimal TillLastRAQty { get; set; }

    [Range(0, float.MaxValue)]
    public decimal CurrentRAQty { get; set; }

    [MaxLength(250)]
    public string Remarks { get; set; }

    public decimal AvailableQty => MeasuredQty - TillLastRAQty;

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
