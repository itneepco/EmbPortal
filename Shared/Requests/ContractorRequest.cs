using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class ContractorRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
