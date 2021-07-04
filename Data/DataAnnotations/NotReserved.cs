using System.ComponentModel.DataAnnotations;

namespace TaskManager.Data.DataAnnotations
{
    public class NotReserved : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string data = value as string;

            if (data is not null && data.Contains("admin") is false)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}