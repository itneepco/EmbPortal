using EmbPortal.Shared.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class FileUploadRequest
    {
        [Required]
        public string FileName { get; set; }
        
        [Required]
        public byte[] FileContent { get; set; }
    }
}
