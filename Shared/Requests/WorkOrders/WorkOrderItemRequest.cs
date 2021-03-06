using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class WorkOrderItemRequest
    {
        [Required, MaxLength(250)]
        public string Description { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a uom")]
        public int UomId { get; set; }

        public string Uom { get; set; }

        [Required, Range(1, double.MaxValue, ErrorMessage = "Please enter a non zero value")]
        public decimal UnitRate { get; set; }

        [Required, Range(float.Epsilon, float.MaxValue, ErrorMessage = "Please enter a non zero value")]
        public float PoQuantity { get; set; }
    }
}

