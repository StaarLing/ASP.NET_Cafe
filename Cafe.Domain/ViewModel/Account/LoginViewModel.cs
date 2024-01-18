using System.ComponentModel.DataAnnotations;

namespace Cafe.Domain.ViewModel.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [Phone(ErrorMessage = "Некоректный номер телефона")]
        [MinLength(12, ErrorMessage = "Некоректный номер телефона")]
        [MaxLength(12,ErrorMessage = "Некоректный номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}