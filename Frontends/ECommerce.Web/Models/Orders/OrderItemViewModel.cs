using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Web.Models.Orders
{
    public class OrderItemViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public Decimal Price { get; set; }
    }
}