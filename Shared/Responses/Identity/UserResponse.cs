using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{DisplayName}, {Designation} ({EmployeeCode})";
        }

        public string LongName => $"{DisplayName} ({EmployeeCode})";
    }
}
