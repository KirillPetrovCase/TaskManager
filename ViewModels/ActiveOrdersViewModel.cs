using System.Collections.Generic;

namespace TaskManager.ViewModels
{
    public class ActiveOrdersViewModel
    {
        public List<ShowOrderViewModel> YourActiveOrders { get; set; }
        public List<ShowOrderViewModel> OtherActiveOrders { get; set; }
    }
}