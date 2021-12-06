using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class MeasurementBookResponse
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string Title { get; set; }
        public string MeasurementOfficer { get; set; }
        public string ValidatingOfficer { get; set; }
        public string Status { get; set; }
        public IReadOnlyList<MBookItemResponse> Items { get; set; }
    }
}
