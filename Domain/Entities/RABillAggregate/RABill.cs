using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.RABillAggregate
{
    public class RABill : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public RABillStatus Status { get; private set; }
        public DateTime BillDate { get; private set; }
        public DateTime ApprovalDate { get; private set; }
        public int MeasurementBookId { get; private set; }
        public MeasurementBook MeasurementBook { get; private set; }

        private readonly List<RABillItem> _items = new List<RABillItem>();
        public IReadOnlyList<RABillItem> Items => _items.AsReadOnly();

        public RABill()
        {
        }

        public RABill(string title, DateTime billDate, int mBookId)
        {
            Title = title;
            BillDate = billDate;
            MeasurementBookId = mBookId;
            Status = RABillStatus.CREATED;
        }

        public void AddLineItem(RABillItem item)
        {
            _items.Add(item);
        }

        public void RemoveLineItem(RABillItem item)
        {
            _items.Remove(item);
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

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SetBillDate(DateTime billDate)
        {
            this.BillDate = billDate;
        }
    }
}
