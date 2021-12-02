using System.Collections.Generic;

namespace Shared.Responses
{
    public class MeasurementBookResponse
    {
        public int WorkOrderId { get; set; }
        public string Title { get; set; }
        public string MeasurementOfficer { get; set; }
        public string ValidatingOfficer { get; set; }
        public IReadOnlyList<MBookItemResponse> Items { get; set; }
    }
}
