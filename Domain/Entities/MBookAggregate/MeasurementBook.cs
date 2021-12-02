using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Entities.WorkOrderAggregate;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MeasurementBook : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public int WorkOrderId { get; private set; }
        public WorkOrder WorkOrder { get; private set; }
        public string MeasurementOfficer { get; private set; }
        public string ValidatingOfficer { get; private set; }
        public bool Status { get; private set; }

        private readonly List<MBookItem> _items = new List<MBookItem>();
        public IReadOnlyList<MBookItem> Items => _items.AsReadOnly();

        public MeasurementBook()
        {
        }

        public MeasurementBook(int workOrderId, string measurementOfficer, string validatingOfficer)
        {
            WorkOrderId = workOrderId;
            MeasurementOfficer = measurementOfficer;
            ValidatingOfficer = validatingOfficer;
        }

        public void AddUpdateLineItem(int wOrderItemId, float quantity, int id=0)
        {
            if (id != 0)  // for item update
            {
                var item = _items.FirstOrDefault(p => p.Id == id);
                item.SetWorkOrderItemNo(wOrderItemId);
                item.SetQuantity(quantity);;
            }
            else  // new item
            {
                _items.Add(new MBookItem(wOrderItemId, quantity));
            }
        }

        public void RemoveLineItem(int id)
        {
            var item = _items.SingleOrDefault(p => p.Id == id);

            if (item != null) // if item exists in the list
            {
                _items.Remove(item);
            }
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