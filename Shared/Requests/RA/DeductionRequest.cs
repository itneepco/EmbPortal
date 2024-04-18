using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests;

public class DeductionRequest
{
    [Required, MaxLength(255)]
    public string Description { get; set; }
    public decimal Amount { get; set; }
}
