using Cafe.Domain.Entities;
using Cafe.Domain.ViewModel.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.ViewModel.Basket
{
    public class ItemBasket
    {
        public dishs Dish { get; set; }
        public int Count { get; set; }
    }
}
