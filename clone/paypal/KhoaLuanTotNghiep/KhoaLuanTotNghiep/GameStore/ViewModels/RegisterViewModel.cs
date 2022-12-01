using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username không được để trống")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối thiểu 5 ký tự")]
        public string ConfirmPassword { get; set; }
    }
}
