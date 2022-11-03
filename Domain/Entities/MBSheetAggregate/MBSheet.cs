using Domain.Common;
using Domain.Entities.Identity;
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
        public DateTime MeasurementDate { get; private set; }
        public string MeasurementOfficer { get; private set; }
        public AppUser Measurer { get; private set; }

        public DateTime ValidationDate { get; private set; }
        public string ValidationOfficer { get; private set; }
        public AppUser Validator { get; private set; }

        public DateTime AcceptingDate { get; private set; }
        public string AcceptingOfficer { get; private set; }
        public AppUser Acceptor { get; set; }

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

        public void RemoveLineItem(MBSheetItem item)
        {
            _items.Remove(item);
        }

        public void MarkAsAccepted()
        {
            Status = MBSheetStatus.ACCEPTED;
            AcceptingDate = DateTime.Now;
        }
        public void MarkPublished()
        {
            Status = MBSheetStatus.PUBLISHED;
        }

        public void MarkAsValidated()
        {
            Status = MBSheetStatus.VALIDATED;
            ValidationDate = DateTime.Now;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetMeasurementDate(DateTime date)
        {
            MeasurementDate = date;
        }
    }
}
