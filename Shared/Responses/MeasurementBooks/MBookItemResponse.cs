using System;

namespace EmbPortal.Shared.Responses;
public class MBookItemResponse
{
    public int Id { get; set; }
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
    public decimal AcceptedMeasuredQty { get; set; }
    public decimal CumulativeMeasuredQty { get; set; }
    public decimal TillLastRAQty { get; set; }

    public decimal TotalAmount
    {
        get
        {
            var amt = (decimal)PoQuantity * UnitRate;
            return decimal.Round(amt, 2, MidpointRounding.AwayFromZero);               
        }
    }

    public int MeasurementProgess
    {
        get
        {
            return (int)Math.Round(CumulativeMeasuredQty / PoQuantity*100);
        }
    }

    public decimal BalanceQuantity
    {
        get
        {
            return PoQuantity - CumulativeMeasuredQty;
        }
    }
}
