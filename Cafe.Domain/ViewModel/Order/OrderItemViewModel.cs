using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.ViewModel.Order
{
    public class OrderItemViewModel
    {
        public dishs Dishs { get; set; }
        public int Count { get; set; }
    }
}
