using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Validations
{
    public class LessThanEqualToValidationAttribute : ValidationAttribute
    {
        public float GivenValue { get; set; }
        public LessThanEqualToValidationAttribute(float value)
        {
            GivenValue = value;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((float)value < GivenValue)
            {
                return new ValidationResult(GetErrorMessage(), new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Please enter value less than {GivenValue}";
        }
    }
}
