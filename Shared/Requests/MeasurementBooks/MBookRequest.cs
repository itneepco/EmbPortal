using EmbPortal.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class MBookRequest
    {
        [Required]
        public int WorkOrderId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, EmployeeCode, Display(Name = "Measurer")]
        public string MeasurementOfficer { get; set; }

        [Required, EmployeeCode, Display(Name = "Validator")]
        public string ValidatingOfficer { get; set; }
    }
}
