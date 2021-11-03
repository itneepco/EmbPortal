using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Uom : AuditableEntity
    {
        public int Id { get; private set; }
        public string Name { get;  set; }
        public UomDimension Dimension {get; set;}
        private Uom()
        {
        }
        public Uom(string name,UomDimension dimension)
        {
            Name = name;
            Dimension = dimension;
        }
    }
}