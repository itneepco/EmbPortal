using EmbPortal.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmbPortal.Shared.Responses
{
    public class RABillDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RABillStatus Status { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string AcceptingOfficer { get; set; }
        public int MeasurementBookId { get; set; }
        public List<RABillItemResponse> Items { get; set; }
        public List<RADeductionResponse> Deductions { get; set; }

        public decimal RABillTotalAmount => Items.Aggregate((decimal)0, (curr, item) => curr + item.CurrentRAAmount);
    }
}
