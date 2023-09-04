using Domain.Common;
using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheet : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public int WorkOrderId { get; private set; }
        public int MeasurementBookId { get; private set; }

        public string Title { get; set; }
        public DateTime MeasurementDate { get; private set; }
        public string MeasurerEmpCode { get; private set; }        

        public DateTime ValidationDate { get; private set; }
        public string ValidatorEmpCode { get; private set; }
        
        public DateTime AcceptingDate { get; private set; }
        public string EicEmpCode { get; private set; }     
        
        public MBSheetStatus Status { get; private set; }

        private readonly List<MBSheetItem> _items = new List<MBSheetItem>();
        public IReadOnlyList<MBSheetItem> Items => _items.AsReadOnly();

        public MBSheet()
        {
        }

        public MBSheet(string title, int measurementBookId, int workOrderId, string measurerEmpCode, DateTime measurementDate, string validatorEmpCode, string eicEmpCode)
        {
            Title = title;
            WorkOrderId = workOrderId;
            MeasurementBookId = measurementBookId;
            MeasurerEmpCode = measurerEmpCode;
            MeasurementDate = measurementDate;
            ValidatorEmpCode = validatorEmpCode;
            EicEmpCode = eicEmpCode;
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
