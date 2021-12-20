using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class UomRequest
    {
        [Required, MaxLength(5)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string Description { get; set; }

        [Required, Range(1,3)]
        public int Dimension { get; set; }
    }
}
