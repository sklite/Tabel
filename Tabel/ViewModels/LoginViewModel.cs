using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Tabel.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Пароль")]
        public string Password { get; set; }

        //public bool Success { get; set; }
    }
}