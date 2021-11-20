using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class CreateWorkOrderRequest
    {
        [Required]
        public string WorkOrderNo { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string Title { get; set; }

        public string AggrementNo { get; set; }

        public DateTime AggrementDate { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int ContractorId { get; set; }

        public List<WorkOrderItemRequest> Items { get; set; } = new List<WorkOrderItemRequest>();
    }
}