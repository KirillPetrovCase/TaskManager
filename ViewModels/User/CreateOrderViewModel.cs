using System;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.DataAnnotations;

namespace TaskManager.ViewModels.User
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Deadline(ErrorMessage = "Срок доджен быть корректным")]
        [Display(Name = "Срок")]
        public DateTime Deadline { get; set; }
    }
}