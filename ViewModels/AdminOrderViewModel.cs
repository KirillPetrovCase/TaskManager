using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class AdminOrderViewModel
    {
        public List<Order> OrdersInWork { get; set; }
        public List<Order> OrdersNotInWork { get; set; }
        public List<Order> CompletedOrders{ get; set; }
    }
}