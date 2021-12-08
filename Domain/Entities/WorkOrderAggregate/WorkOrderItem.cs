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
            AddSubItems(subItems);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void AddSubItems(List<SubItem> subItems)
        {
            foreach (var item in subItems)
            {
                _subItems.Add(new SubItem(item.Description, item.UomId, item.UnitRate, item.PoQuantity));
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
    }
}
