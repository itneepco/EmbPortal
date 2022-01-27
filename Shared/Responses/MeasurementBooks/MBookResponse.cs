using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class MBookResponse
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string Title { get; set; }
        public string MeasurementOfficer { get; set; }
        public string ValidatingOfficer { get; set; }
        public string Status { get; set; }
        public List<MBookItemResponse> Items { get; set; }
    }
}
