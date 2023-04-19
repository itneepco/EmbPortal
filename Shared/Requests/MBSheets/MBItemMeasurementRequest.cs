using System;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests;

public class MBItemMeasurementRequest
{
    [Required]
    public int No { get; set; } = 1;

    [Required, MaxLength(100)]
    public string Description { get; set; }

    [Required, RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a numeric number up to 2 decimal places")]
    public string Val1 { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a numeric number up to 2 decimal places")]
    public string Val2 { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a numeric number up to 2 decimal places")]
    public string Val3 { get; set; }

    public float Total
    {
        get
        {
            float total = 0;

            if (!string.IsNullOrEmpty(Val1) && float.Parse(Val1) > 0) { total = float.Parse(Val1); }
            if (!string.IsNullOrEmpty(Val2) && float.Parse(Val2) > 0) { total *= float.Parse(Val2); }
            if (!string.IsNullOrEmpty(Val3) && float.Parse(Val3) > 0) { total *= float.Parse(Val3); }

            total *= No;

            if (total > 0)
            {
                var result = total.ToString("0.00");
                return float.Parse(result);
            }
            else return total;
        }
    }
}
