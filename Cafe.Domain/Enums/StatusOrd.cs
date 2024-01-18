using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cafe.Domain.Enums
{
    public enum StatusOrd
    {
        [Display(Name = "В обработке")]
        Processing = 0,
        [Display(Name = "Принят")]
        Accept = 1,
        [Display(Name = "Готов")]
        Ready = 2,
        [Display(Name = "В доставке")]
        InDeliv = 3,
        [Display(Name = "Готовится")]
        Cooked = 4,
        [Display(Name = "Завершен")]
        Completed = 5,
        [Display(Name = "Ожидает подтверждения")]
        Wait = 6,
    }
}
