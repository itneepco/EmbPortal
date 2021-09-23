using System;

namespace Application.WorkOrders.Response
{
    public class CreateWorkOrderDto 
    {
        public string WorkOrderNo  { get; set; }
        public DateTime OrderDate { get; set; }
        public string  Title { get; set; }
        public string AggrementNo { get;  set; }
        public DateTime AggrementDate { get; set; }
        public int ProjectId { get;  set; }
        public int ContractorId { get;  set; }
    }        
   
}
    