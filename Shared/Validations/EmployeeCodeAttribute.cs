using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EmbPortal.Shared.Validations
{
    public class EmployeeCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
           object value, ValidationContext validationContext)
        {
            string strValue = value as string;
            Regex rx = new Regex(@"^[0-9]{6}$");

            if (!string.IsNullOrEmpty(strValue) && !rx.IsMatch(strValue))
            {
                return new ValidationResult(GetErrorMessage(),
                    new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Please enter 6 digits employee code";
        }
    }
}
