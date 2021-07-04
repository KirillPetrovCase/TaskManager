using System;
using TaskManager.Data.Enums;

namespace TaskManager.ViewModels
{
    public class ShowOrderViewModel
    {
        public string OrderId { get; set; }
        public string Description { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime DeadLine { get; set; }
        public string Message { get; set; }
        public string AuthorId { get; set; }
        public string PerformerId { get; set; }
        public string AuthorName { get; set; }
    }
}