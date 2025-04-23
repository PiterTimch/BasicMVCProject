using System.ComponentModel.DataAnnotations;

namespace BasicMVCProject.Models.User
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Нема куди листи слати :(")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Шось ти попутав")]
        [Display(Name = "Пошта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Мусово це пиши і десь запам'ятай")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
