using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class PurchaseOrder
    {
        public long OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ContractorId { get; set; }
        public string ContractorName { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public int ItemNo { get; set; }
        public string PackageNo { get; set; }
        public string Description { get; set; }
        public string IsDeleted { get; set; }
        public List<ServiceItem> Details { get; set; }
    }

    public class ServiceItem
    {
        public int SubItemNo { get; set; }
        public string SubItemPackageNo { get; set; }
        public long ServiceNo { get; set; }
        
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public decimal UnitRate { get; set; }
        public string Uom { get; set; }
        public float Quantity { get; set; }
        public string IsDeleted { get; set; }
    }
}
