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
    public decimal Total
    {
        get
        {
            decimal total = 0;

            if (!string.IsNullOrEmpty(Val1) && decimal.Parse(Val1) > 0) { total = decimal.Parse(Val1); }
            if (!string.IsNullOrEmpty(Val2) && decimal.Parse(Val2) > 0) { total *= decimal.Parse(Val2); }
            if (!string.IsNullOrEmpty(Val3) && decimal.Parse(Val3) > 0) { total *= decimal.Parse(Val3); }

            total *= No;           
            return total;
        }
    }

}
