using System;

namespace Shared.Response
{
   public class WorkOrderHeaderResponse
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Title { get;  set; }
        public string AggrementNo { get;  set; }
        public DateTime AggrementDate { get; set; }
        public DateTime CompletionDate { get;  set; }
        public bool IsCompleted { get;  set; }        
        public string Project { get;  set; }        
        public string Contractor { get;  set; }
        public string EngineerInCharge { get; set; }
    }
}