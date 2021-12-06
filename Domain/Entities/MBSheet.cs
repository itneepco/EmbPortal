using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MBSheet : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public DateTime MeasurementDate { get; private set; }
        public MBSheetStatus Status { get; private set; }

        private readonly List<MBSheetItem> _items = new List<MBSheetItem>();
        public IReadOnlyList<MBSheetItem> Items => _items.AsReadOnly();
    }
}
