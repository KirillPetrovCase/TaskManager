using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}