using Domain.Common;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrderItem : AuditableEntity
    {
        public int Id { get; private set; }
        public int WorkOrderId { get; private set; }
        public string Description { get; private set; }

        private readonly List<SubItem> _subItems = new List<SubItem>();
        public IReadOnlyList<SubItem> SubItems => _subItems.AsReadOnly();
        public WorkOrder WorkOrder { get; private set; }

        public WorkOrderItem()
        {
        }

        public WorkOrderItem(string description, List<SubItem> subItems)
        {
            Description = description;
            foreach (var item in subItems)
            {
                AddUpdateSubItem(item.Description, item.UomId, item.UnitRate, item.PoQuantity);
            }
        }

        public void AddUpdateSubItem(string description, int uomId, decimal rate, float poQuantity, int id = 0)
        {
            if (id != 0) // for item update
            {
                var item = _subItems.FirstOrDefault(p => p.Id == id);
                item.SetDescription(description);
                item.SetUomId(uomId);
                item.SetPoQuantity(poQuantity);
                item.SetUnitRate(rate);
            }
            else // new item
            {
                _subItems.Add(new SubItem(description, uomId, rate, poQuantity));
            }
        }

        public void SetDescription(string description)
        {
            Description = description;
        }


        public void SetSubItems(List<SubItem> subItems)
        {
            foreach (var item in subItems)
            {
                AddUpdateSubItem(item.Description, item.UomId, item.UnitRate, item.PoQuantity, item.Id);
            }
        }

        public void RemoveSubItem(int id)
        {
            var item = _subItems.SingleOrDefault(p => p.Id == id);

            if (item != null) // if item exists in the list
            {
                _subItems.Remove(item);
            }
        }

        public void RemoveAllSubItems()
        {
            _subItems.RemoveAll(p => true);
        }


        public float Quantity
        {
            get
            {
                return _subItems.Aggregate(0, (float acc, SubItem item) => acc + item.PoQuantity);
            }
        }

        public float Amount
        {
            get
            {
                return _subItems.Aggregate(0, (float acc, SubItem item) => acc + (item.PoQuantity*(float)item.UnitRate));
            }
        }

    }
}
