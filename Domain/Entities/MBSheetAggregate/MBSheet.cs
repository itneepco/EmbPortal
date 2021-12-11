using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheet : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public string MeasurementOfficer { get; private set; }
        public DateTime MeasurementDate { get; private set; }
        public string ValidationOfficer { get; private set; }
        public DateTime ValidationDate { get; private set; }
        public string AcceptingOfficer { get; private set; }
        public DateTime AcceptingDate { get; private set; }
        public MBSheetStatus Status { get; private set; }

        private readonly List<MBSheetItem> _items = new List<MBSheetItem>();
        public IReadOnlyList<MBSheetItem> Items => _items.AsReadOnly();
    }
}
