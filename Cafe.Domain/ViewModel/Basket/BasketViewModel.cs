using Cafe.Domain.Entities;
using Cafe.Domain.ViewModel.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.ViewModel.Basket
{
    public class BasketViewModel
    {
        public List<ItemBasket> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceWithSell { get; set; }
    }
}
