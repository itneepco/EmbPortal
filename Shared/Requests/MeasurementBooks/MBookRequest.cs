using EmbPortal.Shared.Validations;
using System.Collections.Generic;
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

        public List<MBookItemRequest> Items { get; set; } = new List<MBookItemRequest>();
    }
}
