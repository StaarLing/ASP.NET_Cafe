using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Entities
{
    public class basket
    {
        [Key]
        public int idbus { get; set; }
        public int userid { get; set; }
        public users User { get; set; }
        public List<dishlist> DishList { get; set; }
        
    }
}
