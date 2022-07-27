using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class MBookDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string MeasurementOfficer { get; set; }
        public UserResponse Measurer { get; set; }
        
        public string ValidatingOfficer { get; set; }
        public UserResponse Validator { get; set; }

        public string Status { get; set; }
        public WorkOrderResponse WorkOrder { get; set; }
        public List<MBookItemResponse> Items { get; set; }
    }
}
