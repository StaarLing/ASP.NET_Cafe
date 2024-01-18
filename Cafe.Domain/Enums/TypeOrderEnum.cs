using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cafe.Domain.Enums
{
    public enum TypeOrderEnum
    {
        [Display(Name = "Доставка")]
        Delivery = 0,
        [Display(Name = "В ресторане")]
        InRest = 1

    }
}
