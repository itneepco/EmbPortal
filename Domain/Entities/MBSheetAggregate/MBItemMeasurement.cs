using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MBSheetAggregate;

public class MBItemMeasurement : AuditableEntity
{
    public int Id { get; private set; }
    public int MBSheetItemId { get; set; }
    public string Description { get; set; }
    public int No { get; set; } = 1;
    public string Val1 { get; set; }
    public string Val2 { get; set; }
    public string Val3 { get; set; }

    [NotMapped]
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
