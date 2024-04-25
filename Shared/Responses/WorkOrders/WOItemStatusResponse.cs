namespace EmbPortal.Shared.Responses.WorkOrders;
public class WOItemStatusResponse
{
    public int WorkOrderItemId { get; set; }
    public int ItemNo { get; set; }
    public string PackageNo { get; set; }
    public string ItemDescription { get; set; }
    public int SubItemNo { get; set; }
    public string SubItemPackageNo { get; set; }
    public long ServiceNo { get; set; }
    public string ShortServiceDesc { get; set; }
    public string Uom { get; set; }
    public decimal UnitRate { get; set; }
    public decimal PoQuantity { get; set; }
    public decimal MeasuredQty { get; set; }   
    public decimal TillLastRAQty { get; set; }
    public decimal AvailableQty
    {
        get
        {
            return MeasuredQty - TillLastRAQty;
        }
    }
}
