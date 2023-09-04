using System;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderResponse
    {
        public int Id { get; set; }
        public long OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Project { get; set; }
        public string Contractor { get; set; }
        public string EicEmployeeCode { get; set; }
        public string EicFullName { get; set; }        
    }
}