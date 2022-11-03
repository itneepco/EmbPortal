using System;

namespace EmbPortal.Shared.Responses
{
    public class MBSheetInfoResponse
    {
        public int Id { get; set; }
        public int MeasurementBookId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime MeasurementDate { get; set; }
        public DateTime ValidationDate { get; set; }
        public string MBookTitle { get; set; }
    }
}
