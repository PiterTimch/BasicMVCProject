using System.ComponentModel.DataAnnotations;

namespace BasicMVCProject.Models.User
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "У тебе що, імені нема?")]
        [DataType(DataType.Text)]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Чий будеш?")]
        [DataType(DataType.Text)]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не можна без клікухи")]
        [DataType(DataType.Text)]
        [Display(Name = "Клікуха")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Нема куди листи слати :(")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Шось ти попутав")]
        [Display(Name = "Пошта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Куди альо робити?")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Мобілка")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Мусово це пиши і десь запам'ятай")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Фотка")]
        public IFormFile? ImageFile { get; set; } = null;
    }
}
