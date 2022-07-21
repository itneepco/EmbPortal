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

        [Required, MaxLength(6)]
        public string EngineerInCharge { get; set; }

        [Required]
        public string Project { get; set; }

        [Required]
        public string Contractor { get; set; }
    }
}
