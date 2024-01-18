using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Entities
{
    public class orderbasket
    {
        [Key]
        public int IdOrderBas { get; set; }
        public int UserId { get; set; }
        public users User { get; set; }
        public List<order> orderlists { get; set; }
    }
}
