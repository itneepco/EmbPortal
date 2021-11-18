using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class ContractorRequest
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
