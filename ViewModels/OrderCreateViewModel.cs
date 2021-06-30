using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        public string Description { get; set; }

        public DateTime DeadLine { get; set; }
    }
}