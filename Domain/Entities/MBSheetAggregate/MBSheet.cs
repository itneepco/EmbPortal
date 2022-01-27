using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheet : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string MeasurementOfficer { get; private set; }
        public DateTime MeasurementDate { get; private set; }
        public string ValidationOfficer { get; private set; }
        public DateTime ValidationDate { get; private set; }
        public string AcceptingOfficer { get; private set; }
        public DateTime AcceptingDate { get; private set; }
        public MBSheetStatus Status { get; private set; }

        public int MeasurementBookId { get; private set; }
        public MeasurementBook MeasurementBook { get; private set; }

        private readonly List<MBSheetItem> _items = new List<MBSheetItem>();
        public IReadOnlyList<MBSheetItem> Items => _items.AsReadOnly();

        public MBSheet()
        {
        }

        public MBSheet(string title, int measurementBookId, string measurementOfficer, DateTime measurementDate, string validationOfficer, string acceptingOfficer)
        {
            Title = title;
            MeasurementBookId = measurementBookId;
            MeasurementOfficer = measurementOfficer;
            MeasurementDate = measurementDate;
            ValidationOfficer = validationOfficer;
            AcceptingOfficer = acceptingOfficer;
            Status = MBSheetStatus.CREATED;
        }

        public void AddLineItem(MBSheetItem item)
        {
            _items.Add(item);
        }

        public void MarkAsAccepted()
        {
            Status = MBSheetStatus.ACCEPTED;
            AcceptingDate = new DateTime();
        }

        public void MarkAsValidated()
        {
            Status = MBSheetStatus.VALIDATED;
            ValidationDate = new DateTime();
        }
    }
}
