using Cafe.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Domain.Entities
{
    public class users
    {
        [Key]
        public int IdU { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Role idR { get; set; }
        public profile Profile { get; set; }
        public basket Basket { get; set; }
        public orderbasket OrderBasket { get; set; }

    }

}
