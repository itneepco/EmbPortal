using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class WorkOrderItemRequest
    {
        [Required, MaxLength(250)]
        public string Description { get; set; }

        public List<SubItemRequest> SubItems { get; set; }
    }
}
