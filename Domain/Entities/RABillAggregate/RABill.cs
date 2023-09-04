using Domain.Common;
using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.RABillAggregate;
public class RABill : AuditableEntity, IAggregateRoot
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public RABillStatus Status { get; private set; }
    public DateTime BillDate { get; private set; }
    public DateTime ApprovalDate { get; private set; }
    public string EicEmpCode { get; private set; }       

    public int MeasurementBookId { get; private set; }
    public int WorkOrderId { get; set; }

    private readonly List<RABillItem> _items = new List<RABillItem>();
    public IReadOnlyList<RABillItem> Items => _items.AsReadOnly();

    private readonly List<RADeduction> _deductions = new List<RADeduction>();
    public IReadOnlyList<RADeduction> Deductions => _deductions.AsReadOnly();

    public RABill()
    {
    }

    public RABill(string title, DateTime billDate, int mBookId, string eicEmpCode, int workOrderId)
    {
        Title = title;
        BillDate = billDate;
        MeasurementBookId = mBookId;
        Status = RABillStatus.CREATED;
        EicEmpCode = eicEmpCode;
        WorkOrderId = workOrderId;
    }

    public void AddLineItem(RABillItem item)
    {
        _items.Add(item);
    }

    public void RemoveLineItem(RABillItem item)
    {
        _items.Remove(item);
    }

    public void AddDeduction(RADeduction deduction)
    {
        _deductions.Add(deduction);
    }

    public void RemoveDeduction(RADeduction deduction)
    {
        _deductions.Remove(deduction);
    }

    public void MarkAsApproved()
    {
        Status = RABillStatus.APPROVED;
        ApprovalDate = DateTime.Now;
    }

    public void MarkAsRevoked()
    {
        Status = RABillStatus.REVOKED;
    }
    public void MarkAsPosted()
    {
        Status = RABillStatus.POSTED;
    }
    public void SetTitle(string title)
    {
        this.Title = title;
    }

    public void SetEicEmpCode(string eicEmpCode)
    {
        this.EicEmpCode = eicEmpCode;
    }

    public void SetBillDate(DateTime billDate)
    {
        this.BillDate = billDate;
    }
}
