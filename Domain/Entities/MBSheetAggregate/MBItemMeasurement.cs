using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MBSheetAggregate;

public class MBItemMeasurement : AuditableEntity
{
    public int Id { get; private set; }
    public int MBSheetItemId { get; set; }
    public string Description { get; set; }
    public int No { get; set; } = 1;
    public float Val1 { get; set; }
    public float Val2 { get; set; }
    public float Val3 { get; set; }
    [NotMapped]
    public float Total
    {
        get
        {
            float total = No;
            if (Val1 > 0) { total *= Val1; }
            if (Val2 > 0) { total *= Val2; }
            if (Val3 > 0) { total *= Val3; }

            return total;
        }
    }

}
