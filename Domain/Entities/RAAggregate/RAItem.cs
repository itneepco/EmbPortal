using Domain.Common;
namespace Domain.Entities.RAAggregate;
public class RAItem : AuditableEntity
{
    public int Id { get; private set; }   
    public string Uom { get; set; }
    public decimal UnitRate { get; set; }
    public decimal PoQuantity { get; set; }
    public decimal MeasuredQty { get;  set; }
    public decimal TillLastRAQty { get;  set; }
    public decimal CurrentRAQty { get;  set; }
    public string Remarks { get;  set; } // character 100 max limit
    public int WorkOrderItemId { get;  set; }
    public int ItemNo { get; set; }
    public string ItemDescription { get; set; }
    public string PackageNo { get; set; }
    public int SubItemNo { get; set; }
    public string SubItemPackageNo { get; set; }
    public long ServiceNo { get; set; }
    public string ShortServiceDesc { get; set; }    

    public RAItem()
    {
    }
}
