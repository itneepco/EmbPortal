﻿using Domain.Common;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheetItem :AuditableEntity
    {
        public int Id { get; set; }
        public int SubItemId { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }
    }
}
