using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses.RA;

public class RAReportResponse
{
    public string RaTitle { get; set; }
    public DateTime BillDate { get; set; }
    public long PoNo { get; set; }    
    public string Contractor { get; set; }
    public string Eic { get; set; }    
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime? ActualCompletionDate { get; set; }
    public string LastBill { get; set; }
    public string Remarks { get; set; }
    public decimal TotalRaAmount { get; set; }
    public decimal TotalDeduction { get; set; }
    public decimal NetRaAmount => TotalRaAmount - TotalDeduction;
    public List<Item> Items { get; set; }
    public List<DeductionView> Deductions { get; set; }
}
public class Item
{
    public int No { get; set; }
    public string Description { get; set; }
    public List<SubItem> SubItems { get; set; }
}

public class SubItem
{
    public int No { get; set; } 
    public string ShortServiceDesc { get; set; }
    public string Uom { get; set; }
    public decimal UnitRate { get; set; }
    public decimal PoQuantity { get; set; }
    public decimal MeasuredQty { get; set; }
    public decimal TillLastRaQty { get; set; }
    public decimal CurrentRaQty { get; set; }
    public decimal RaAmount => CurrentRaQty * UnitRate;
    public string Remarks { get; set; }
}
public class DeductionView
{  
    public string Description { get; set; }
    public decimal Amount { get; set; }
}
