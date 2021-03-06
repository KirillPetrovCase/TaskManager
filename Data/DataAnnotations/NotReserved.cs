using System.ComponentModel.DataAnnotations;

namespace TaskManager.Data.DataAnnotations
{
    public class NotReserved : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string userLogin = value as string;

            if (userLogin is not null && userLogin.Contains("admin") is false)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}