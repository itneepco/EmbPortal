using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
   public class ProjectRequest
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}