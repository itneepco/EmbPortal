using EmbPortal.Shared.Enums;
using System;

namespace EmbPortal.Shared.Responses
{
    public class RABillResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RABillStatus Status { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string EicEmpCode { get; set; }
        public int MeasurementBookId { get; set; }
    }
}
