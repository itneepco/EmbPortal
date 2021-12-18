using EmbPortal.Shared.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class UserRequest
    {
        [Required, Display(Name = "Display Name"), MaxLength(50)]
        public string DisplayName { get; set; }

        [Required, EmployeeCode, Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string Designation { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
