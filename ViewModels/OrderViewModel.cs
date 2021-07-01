using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name = "Описание проблемы")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Срок")]
        public DateTime DeadLine { get; set; }
    }
}