using Cafe.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Domain.Entities
{
    public class dishs
    {
        [Key]
        public int IdD { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public int Sell { get; set; }
        public decimal Price { get; set; }
        public Category IdK { get; set; }
        public byte[]? Avatar { get; set; }
        public int Count { get; set; }
    }
}
