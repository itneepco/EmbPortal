using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Requests
{
    public class RABillRequest
    {
        public string Title { get; set; }
        public DateTime? BillDate { get; private set; }
        public List<RABillItemRequest> Items { get; set; } = new();
    }
}
