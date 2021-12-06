using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class UomRequest
    {
        [Required]
        public string Name { get; set; }

        [Range(1,3)]
        public int Dimension { get; set; }
    }
}
