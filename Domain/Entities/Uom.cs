using Domain.Common;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Uom : AuditableEntity
    {
        public int Id { get; private set; }
        public string Name { get;  set; }
        public string Description { get; set; }
        public UomDimension Dimension { get; set; }
        //public IReadOnlyList<WorkOrderItem> WorkOrderItems { get; set; }

        private Uom()
        {
        }

        public Uom(string name, string description, UomDimension dimension)
        {
            Name = name;
            Description = description;
            Dimension = dimension;
        }
    }
}