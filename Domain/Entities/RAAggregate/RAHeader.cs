using Domain.Common;

using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.RAAggregate;

public class RAHeader : AuditableEntity, IAggregateRoot
{
    public int Id { get; private set; }
    public string Title { get;  set; }
    public RAStatus Status { get;   set; }
    public DateTime BillDate { get;  set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public string LastBillDetail { get; set; } = string.Empty;   
    public string EicEmpCode { get;  set; }    
    public int WorkOrderId { get; set; }
    private readonly List<RAItem> _items = new List<RAItem>();
    public IReadOnlyList<RAItem> Items => _items.AsReadOnly();
    private readonly List<Deduction> _deductions = new List<Deduction>();
    public IReadOnlyList<Deduction> Deductions => _deductions.AsReadOnly();
    public void AddItem(RAItem item)
    {
        _items.Add(item);
    }
    public void RemoveItem(RAItem item)
    {
        _items.Remove(item);
    }
    public void AddDeduction(Deduction deduction)
    {
        _deductions.Add(deduction);
    }
    public void RemoveDeduction(Deduction deduction)
    {
        _deductions.Remove(deduction);
    }
    public void RemoveAllDeductions()
    {
        _deductions?.Clear();
    }
    public void MarkAsPosted()
    {
        Status = RAStatus.Posted;
    }
}
    