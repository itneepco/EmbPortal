using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class RABillItemRequest : IValidatableObject
    {
        public int MBookItemId { get; set; }
        public string MBookItemDescription { get; set; }
        public decimal UnitRate { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }

        [Range(0, float.MaxValue)]
        public float CurrentRAQty { get; set; }

        [MaxLength(100)]
        public string Remarks { get; set; }

        public float AvailableQty => AcceptedMeasuredQty - TillLastRAQty;

        public decimal AvailableAmount => UnitRate * (decimal)AvailableQty;

        public decimal CurrentRAAmount => UnitRate * (decimal)CurrentRAQty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(CurrentRAQty > AvailableQty)
            {
                yield return new ValidationResult(GetErrorMessage(), new[] { nameof(CurrentRAQty) });
            }
        }

        private string GetErrorMessage()
        {
            return $"Current RA Qty {CurrentRAQty} must be less than or equal to {AvailableQty.ToString("0.00")}";
        }
    }
}
