using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Domain.Entities
{
    public class order
    {
        [Key]
        public int idorder { get; set; }
        public int orderbasketid { get; set; }
        public virtual orderbasket Orderbasket { get; set; }
        public List<orderitem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public TypeOrderEnum TypeId { get; set; }
        public StatusOrd StatusId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
