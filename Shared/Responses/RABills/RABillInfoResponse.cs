using System;

namespace EmbPortal.Shared.Responses
{
    public class RABillInfoResponse
    {
        public int Id { get; set; }
        public int MeasurementBookId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Contractor { get; set; }
        public string MBookTitle { get; set; }
        public string RABillTitle { get; set; }
    }
}
