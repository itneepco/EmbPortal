using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class WorkOrderRequest
    {
        [Required]
        public string WorkOrderNo { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string Title { get; set; }

        public string AgreementNo { get; set; }

        public DateTime AgreementDate { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int ContractorId { get; set; }
    }
}
