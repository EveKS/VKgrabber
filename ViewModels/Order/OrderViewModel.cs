using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.ViewModels.Order
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public decimal LastOrdered { get; set; }
        public decimal Sum { get; set; }
        public bool IsNotOrdered { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
