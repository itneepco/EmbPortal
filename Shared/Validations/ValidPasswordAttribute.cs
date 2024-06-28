using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EmbPortal.Shared.Validations;

public class ValidPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
       object value, ValidationContext validationContext)
    {
        string strValue = value as string;
        Regex rx = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

        if (!string.IsNullOrEmpty(strValue) && !rx.IsMatch(strValue))
        {
            return new ValidationResult(GetErrorMessage(), [validationContext.MemberName]);
        }

        return ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        return $"Password should contain least 8 characters, " +
            $"both lower & uppercase letters, atleast one number and special characters";
    }
}
