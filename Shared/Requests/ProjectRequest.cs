using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
   public class ProjectRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}