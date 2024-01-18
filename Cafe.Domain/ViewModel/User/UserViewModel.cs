using Cafe.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Domain.ViewModel.User
{

    public class UserViewModel
    {
        [Display(Name = "Id")]
        public int IdU { get; set; }

        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Display(Name = "Номер телефона")]
        [Phone]
        public string Phone { get; set; }
    }
}

