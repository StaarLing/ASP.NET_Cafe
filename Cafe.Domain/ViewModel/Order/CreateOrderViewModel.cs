using Cafe.Domain.Entities;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cafe.Domain.ViewModel.Order
{
    public class CreateOrderViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Укажите адрес")]
        [MinLength(5, ErrorMessage = "Адрес должен быть больше 5 символов")]
        [MaxLength(200, ErrorMessage = "Адрес должен быть меньше 200 символов")]
        public string Address { get; set; }
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Введите имя пользователя")]
        [MinLength(5, ErrorMessage = "Некоректное имя пользователя")]
        [MaxLength(50, ErrorMessage = "Некоректное имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Введите номер телефона")]
        [Phone(ErrorMessage = "Некоректный номер телефона")]
        [MinLength(13, ErrorMessage = "Некоректный номер телефона")]
        [MaxLength(13, ErrorMessage = "Некоректный номер телефона")]
        public string Phone { get; set; }
        public List<DishViewModel> Dishs { get; set; }
        public string Type { get; set; }
    }
}
