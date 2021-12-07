using System;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class WorkOrderRequest
    {
        [Required, MaxLength(250)]
        public string Title { get; set; }

        [Required, MaxLength(60)]
        public string OrderNo { get; set; }

        [Required]
        public DateTime? OrderDate { get; set; }

        [Required, MaxLength(60)]
        public string AgreementNo { get; set; }

        [Required]
        public DateTime? AgreementDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a project"), Required]
        public int ProjectId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a contractor"), Required]
        public int ContractorId { get; set; }
    }
}
