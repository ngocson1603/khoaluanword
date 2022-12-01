using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối thiểu 5 ký tự")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối thiểu 5 ký tự")]
        public string ConfirmPassword { get; set; }
    }
}
