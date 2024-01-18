using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cafe.Domain.Enums
{
    public enum Category
    {
        [Display(Name = "Горячие блюда")]
        First = 0,
        [Display(Name = "Салаты")]
        Second = 1,
        [Display(Name = "Напитки")]
        Drink = 2
    }
}
