using System;
using System.Collections.Generic;
using System.Linq;

namespace EmbPortal.Shared.Responses.RA;

public class RAReportResponse
{
    public string RABillTitle { get; set; }
    public DateTime BillDate { get; set; }
    public string PoNo { get; set; }   
    public string MBTitle { get; set; }
    public string Contractor { get; set; }
    public string EIC { get; set; }
    public string ValidationOfficer { get; set; }
    public string MeaurementOfficer { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime? ActualCompletionDate { get; set;}
    public string LastBill { get; set; }
    public string Remarks { get; set;}
    public List<RAItemResponse> RABillItems { get; set; } 
    public IReadOnlyList<RADeductionResponse> Deductions { get; set; }
    public decimal TotalAmount => RABillItems.Aggregate((decimal)0, (curr, item) => curr + item.CurrentRAAmount);
    public decimal TotalDeduction => Deductions.Aggregate((decimal)0, (curr, item) => curr + item.Amount);
    public decimal NetAmount => TotalAmount - TotalDeduction;
}


