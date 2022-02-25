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
    }
}
