using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Entities.WorkOrderAggregate;
using Domain.Enums;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MeasurementBook : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public int WorkOrderId { get; private set; }
        public string Title { get; private set; }
        public string MeasurementOfficer { get; private set; }
        public string ValidatingOfficer { get; private set; }
        public MBookStatus Status { get; private set; }
        public WorkOrder WorkOrder { get; private set; }

        private readonly List<MBookItem> _items = new List<MBookItem>();
        public IReadOnlyList<MBookItem> Items => _items.AsReadOnly();

        public MeasurementBook()
        {
        }

        public MeasurementBook(int workOrderId, string title, string measurementOfficer, string validatingOfficer)
        {
            Title = title;
            WorkOrderId = workOrderId;
            MeasurementOfficer = measurementOfficer;
            ValidatingOfficer = validatingOfficer;
            Status = MBookStatus.CREATED;
        }

        public void AddUpdateLineItem(int wOrderItemId, int id=0)
        {
            if (Status == MBookStatus.PUBLISHED || Status == MBookStatus.COMPLETED) return;

            if (id != 0)  // for item update
            {
                var item = _items.FirstOrDefault(p => p.Id == id);
                item.SetWorkOrderItemNo(wOrderItemId);
            }
            else  // new item
            {
                _items.Add(new MBookItem(wOrderItemId));
            }
        }

        public void RemoveByOrderItemId(int orderItemid)
        {
            if (Status == MBookStatus.PUBLISHED || Status == MBookStatus.COMPLETED) return;

            var item = _items.SingleOrDefault(p => p.WorkOrderItemId == orderItemid);

            if (item != null) // if item exists in the list
            {
                _items.Remove(item);
            }
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetWorkOrderId(int workOrderId)
        {
            WorkOrderId = workOrderId;
        }

        public void SetMeasurementOfficer(string measurementOfficer)
        {
            MeasurementOfficer = measurementOfficer;
        }

        public void SetValidatingOfficer(string validatingOfficer)
        {
            ValidatingOfficer = validatingOfficer;
        }
    }
}