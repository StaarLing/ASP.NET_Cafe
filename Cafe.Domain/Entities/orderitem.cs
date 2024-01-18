using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Entities
{
    public class orderitem
    {
        [Key]
        public int IdOrderItem { get; set; }
        public int DishId { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public virtual order OrderList { get; set; }
    }
}
