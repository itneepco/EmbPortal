using EmbPortal.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class WorkOrderRequest
    {
        [Required]
        public long OrderNo { get; set; }

        [Required, EmployeeCode]
        [Display(Name = "Engineer In Charge")]
        public string EngineerInCharge { get; set; }

    }
}
