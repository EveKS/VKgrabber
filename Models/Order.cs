using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.Models
{
    public class Order
    {
        public string OrderId { get; set; } // id заказа

        public DateTime? Date { get; set; } // дата
        public DateTime? DateEnd { get; set; } // дата до
        public decimal Sum { get; set; } // сумма заказа
        public decimal? Payed { get; set; } // оплачено

        public bool IsOrdered { get; set; } // оплачено-ли

        public string Sender { get; set; } // отправитель - кошелек в ЯД
        public string Operation_Id { get; set; } // id операции в ЯД
        public decimal? Amount { get; set; } // сумма, которую заплатали с учетом комиссии
        public decimal? WithdrawAmount { get; set; } // сумма, которую заплатали без учета комиссии
        public int? UserId { get; set; } // id пользователя в системе, который сделал заказ
    }
}
