using System;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderResponse
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Project { get; set; }
        public string Contractor { get; set; }
        public string EngineerInCharge { get; set; }
        public UserResponse Engineer { get; set; }
        public string Status { get; set; }
    }
}