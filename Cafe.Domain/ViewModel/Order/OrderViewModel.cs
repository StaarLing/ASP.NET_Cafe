using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.ViewModel.Order
{
    public class OrderViewModel
    {
        public int id { get; set; }
        public DateTime DateCreate { get; set; }
        [DisplayName("Введите адрес")]
        [MinLength(4,ErrorMessage ="Слишком короткий адрес")]
        [MaxLength(50, ErrorMessage = "Слишком длинный адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [Phone]
        [DisplayName("Введите номер телефона")]
        public string Phone { get; set; }
        public List<OrderItemViewModel> Dishs { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Тип заказа")]
        public string Type { get; set; }
        public string Status { get; set; }

    }
}
