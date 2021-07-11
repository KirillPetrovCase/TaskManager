using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Data.DataAnnotations
{
    public class Deadline : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = value as DateTime?;

            if (date is not null && date > DateTime.Now)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}