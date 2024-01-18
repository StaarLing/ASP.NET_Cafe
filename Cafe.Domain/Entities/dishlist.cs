using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Entities
{
    public class dishlist
    {
        [Key]
        public int IdList { get; set; }
        public int IdDish { get; set; }
        public int Count { get; set; }
        public int Bus { get; set; }
        public virtual basket basket { get; set; }
    }
}
