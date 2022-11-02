using EmbPortal.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests.MeasurementBooks;

public class ChangeOfficerRequest
{
    [Required, EmployeeCode]
    [Display(Name = "Officer")]
    public string Officer { get; set; }

}
