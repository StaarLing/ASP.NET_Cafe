using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Entities
{
    public class profile
    {
        [Key] 
        public int IdP { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public users User { get; set; }
        public int userid { get; set; }
    }
}
