using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests;

public class DeductionRequest
{
    [Required, MaxLength(250)]
    public string Description { get; set; }
    public decimal Amount { get; set; }
}
