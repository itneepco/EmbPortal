using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class WorkOrderRequest
    {
        [Required]
        public long OrderNo { get; set; }

        [Required, MaxLength(6)]
        [Display(Name = "Engineer In Charge")]
        public string EngineerInCharge { get; set; }

    }
}
