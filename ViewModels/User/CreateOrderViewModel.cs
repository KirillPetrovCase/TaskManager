using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels.User
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Срок")]
        public DateTime Deadline { get; set; }
    }
}