using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class RABillResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RABillStatus Status { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public int MeasurementBookId { get; set; }
        public List<RABillItemResponse> Items { get; set; }
    }
}
