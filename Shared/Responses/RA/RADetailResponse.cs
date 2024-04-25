using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses.RA;

public class RADetailResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string WorkOrderNo { get; set; }
    public RAStatus Status { get; set; }
    public DateTime BillDate { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string Remarks { get; set; } 
    public string LastBillDetail { get; set; }
    public string EicEmpCode { get; set; }
    public decimal TotalRaAmount { get; set; }
    public decimal TotalDeduction { get; set; }    
    public decimal NetRaAmount => TotalRaAmount - TotalDeduction;
    public List<RaItemView> Items { get; set; }
    public List<RaDeductionView> Deductions  { get; set; }
}

public class RaItemView
{
    public int Id { get; set;}
    public int WorkOrderItemId { get; set; }
    public string Uom { get; set; }
    public decimal UnitRate { get; set; }
    public decimal PoQuantity { get; set; }
    public decimal MeasuredQty { get; set; }
    public decimal TillLastRaQty { get; set; }
    public decimal CurrentRaQty { get; set; }
    public string Remarks { get; set; }
    public int ItemNo { get; set; }
    public string ItemDescription { get; set; }
    public int SubItemNo { get; set; }
    public long ServiceNo { get; set; }
    public string ShortServiceDesc { get; set; }
    public decimal RaAmount => (decimal)CurrentRaQty * UnitRate;
}
public class RaDeductionView
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}