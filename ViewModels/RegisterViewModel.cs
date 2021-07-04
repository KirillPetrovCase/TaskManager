using System.ComponentModel.DataAnnotations;
using TaskManager.Data.DataAnnotations;

namespace TaskManager.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Логин")]
        [NotReserved (ErrorMessage = "Этот логин зарезервирован")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Должность")]
        public string Post { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Отдел")]
        public string Placement { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Минимальная длина пароля 5 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадаюсь")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}