using EmbPortal.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Identity
{
    public class UserFormDto
    {
        [Required, Display(Name = "Display Name")]
        [MaxLength(50)]
        public string DisplayName { get; set; }

        [Required, Display(Name = "Employee Code")]
        [EmployeeCode]
        public string EmployeeCode { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RegisterDto : UserFormDto
    {
        [Required]
        [DataType(DataType.Password)]
        [ValidPassword]
        public string Password { get; set; }
    }

    public class UpdateUserDto : UserFormDto
    {
        public string Id { get; set; }
    }
}
