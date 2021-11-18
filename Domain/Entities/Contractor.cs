using Domain.Common;
using Domain.Entities.WorkOrderAggregate;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Contractor : AuditableEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyList<WorkOrder> WorkOrders { get; set; }

        private Contractor() { }

        public Contractor(string name)
        {
            Name = name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

    }
}
