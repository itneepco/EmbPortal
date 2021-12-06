using System;
using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
   public class WorkOrderResponse
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }      
        public string Title { get; set; }
        public string AgreementNo { get; set; }
        public DateTime AgreementDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string EngineerInCharge { get; set; }
        public string Status { get; set; }
        public IReadOnlyList<WorkOrderItemResponse> Items { get; set; }
    }
}