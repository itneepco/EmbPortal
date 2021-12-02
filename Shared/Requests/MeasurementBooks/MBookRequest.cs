using Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class MBookRequest
    {
        [Required]
        public int WorkOrderId { get; set; }

        [Required, EmployeeCode]
        public string MeasurementOfficer { get; set; }

        [Required, EmployeeCode]
        public string ValidatingOfficer { get; set; }
    }
}
