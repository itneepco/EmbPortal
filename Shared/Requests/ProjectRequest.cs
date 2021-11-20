using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
   public class ProjectRequest
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}