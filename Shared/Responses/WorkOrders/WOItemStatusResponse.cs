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
    public float PoQuantity { get; set; }
    public float MeasuredQty { get; set; }   
    public float TillLastRAQty { get; set; }
    public float AvailableQty
    {
        get
        {
            var available = (MeasuredQty - TillLastRAQty).ToString("0.00");
            return float.Parse(available);
        }
    }
}
