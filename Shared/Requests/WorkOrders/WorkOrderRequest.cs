using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class WorkOrderRequest
    {
        [Required, MaxLength(250)]
        public string Title { get; set; }

        [Required, MaxLength(60)]
        public string WorkOrderNo { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [MaxLength(60)]
        public string AgreementNo { get; set; }

        public DateTime AgreementDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ProjectId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ContractorId { get; set; }
    }
}
