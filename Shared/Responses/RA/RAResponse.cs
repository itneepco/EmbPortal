using EmbPortal.Shared.Enums;
using System;

namespace EmbPortal.Shared.Responses.RA;

public class RAResponse
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public string Title { get; set; }
    public RAStatus Status { get; set; }
    public DateTime BillDate { get; set; }

    public decimal RAAmount { get; set; }
    public decimal Deduction { get; set; }
    public decimal NetAmount => RAAmount - Deduction;          
}
