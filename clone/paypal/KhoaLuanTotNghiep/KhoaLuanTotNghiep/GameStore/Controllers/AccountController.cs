using AspNetCoreHero.ToastNotification.Abstractions;
using GameStore.Extensions;
using GameStore.Interfaces;
using GameStore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.Controllers
{
    public class AccountController : Controller
    {
        public INotyfService _notyfService { get; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;

        public AccountController(INotyfService notyfService, IUnitOfWork unitOfWork, IService service)
        {
            _notyfService = notyfService;
            _unitOfWork = unitOfWork;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                int kq = _service.AccountService.SignIn(model);

                if (kq == -1)
                {
                    _notyfService.Information("Không tìm thấy tài khoản được đăng ký với địa chỉ email này!");
                }
                else if (kq == 2)
                {
                    _notyfService.Error("Đăng nhập thất bại!<br>Vui lòng kiểm tra tên đăng nhập và mật khẩu");
                }
                else
                {
                    // Lưu session đăng nhập: SS_AccountId
                    HttpContext.Session.SetString("SS_AccountId", kq.ToString());
                    string name = _unitOfWork.AccountRepository.GetById(kq).Username;

                    // Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, name),
                        new Claim("SS_AccountId", kq.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                    _notyfService.Success($"Đăng nhập thành công!<br>Chào mừng bạn quay trở lại {name}");
                    return RedirectToAction("Dashboard");
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyfService.Warning("Vui lòng kiểm tra thông tin đăng nhập!");
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public IActionResult SignUp(RegisterViewModel model)
        {
            if (model.ConfirmPassword != model.Password)
            {
                _notyfService.Warning("Mật khẩu xác nhận không trùng khớp!");
                return RedirectToAction("Index", "Home");
            }    

            if (ModelState.IsValid)
            {
                int kq = _service.AccountService.SignUp(model);

                if (kq == -1)
                {
                    _notyfService.Information("Địa chỉ email này đã được đăng ký!");
                }
                else if (kq == 0)
                {
                    _notyfService.Error("Đăng ký thất bại!");
                }
                else
                {
                    _unitOfWork.SaveChange();
                    _notyfService.Success("Đăng ký thành công!");
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyfService.Warning("Vui lòng kiểm tra thông tin đăng ký!");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("SS_AccountId");
            return RedirectToAction("HomePage", "Product");
        }

        [Route("my-account.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var accountId = HttpContext.Session.GetString("SS_AccountId");
            var account = _unitOfWork.AccountRepository.GetById(Convert.ToInt32(accountId));

            if (accountId == null || account == null)
            {
                return RedirectToAction("SignIn");
            }

            return View(account);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var accountID = HttpContext.Session.GetString("SS_AccountId");

            if (accountID == null)
            {
                return RedirectToAction("SignIn");
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                _notyfService.Warning("Mật khẩu xác nhận không trùng khớp!");
                return RedirectToAction("Dashboard");
            }

            if (ModelState.IsValid)
            {
                int kq = _service.AccountService.ChangePassword(model, Convert.ToInt32(accountID));

                switch (kq)
                {
                    case -2:
                        _notyfService.Error("Mật khẩu hiện tại không đúng!");
                        break;
                    case -1:
                        _notyfService.Information("Không tìm thấy tài khoản!");
                        break;
                    case 0:
                        _notyfService.Error("Có lỗi trong quá trình đổi mật khẩu!");
                        break;
                    case 1:
                        _unitOfWork.SaveChange();
                        _notyfService.Success("Đổi mật khẩu thành công!");
                        break;

                    default:
                        _notyfService.Error("Lỗi không xác định!");
                        break;
                }

                return RedirectToAction("Dashboard");
            }
            else
            {
                _notyfService.Warning("Vui lòng kiểm tra thông tin thay đổi!");
                return RedirectToAction("Dashboard");
            }
        }
    }
}
