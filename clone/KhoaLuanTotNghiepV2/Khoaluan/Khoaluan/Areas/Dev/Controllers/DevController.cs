using AspNetCoreHero.ToastNotification.Abstractions;
using Khoaluan.Areas.Admin.Models;
using Khoaluan.Areas.Dev.Models;
using Khoaluan.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Khoaluan.Controllers
{
    [Area("Dev")]
    [Authorize]
    public class DevController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notyfService { get; }
        public DevController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _notyfService = notyfService;
        }
        [AllowAnonymous]
        [Route("logindev.html", Name = "Logindev")]
        public IActionResult LoginDev(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null) return RedirectToAction("Index", "Trang1", new { Area = "Dev" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("logindev.html", Name = "Logindev")]
        public async Task<IActionResult> LoginDev(LoginDevViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.IsInRole("User"))
                    {
                        _notyfService.Warning("Vui lòng đăng xuất ở User");
                        return RedirectToAction("Dashboard", "Users");
                    }
                    if (User.IsInRole("Admin"))
                    {
                        _notyfService.Warning("Vui lòng đăng xuất ở Admin");
                        return RedirectToAction("Index", "Home", new { Area = "Dev" });
                    }
                    var kh = _unitOfWork.DeveloperRepository.GetAll().SingleOrDefault(x => x.UserName.Trim() == model.UserName);

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                    }
                    string pass = (model.Password.Trim());
                    // + kh.Salt.Trim()
                    if (kh.Passwork.Trim() != pass)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    //đăng nhập thành công


                    var taikhoanID = HttpContext.Session.GetString("AccountId");
                    //identity
                    //luuw seccion Makh
                    HttpContext.Session.SetString("AccountId", kh.Id.ToString());
                    HttpContext.Session.SetString("Role", "Dev");
                    var Roles = HttpContext.Session.GetString("Role");
                    //identity
                    var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, kh.Name),
                            new Claim(ClaimTypes.Email, kh.UserName),
                            new Claim("AccountId", kh.Id.ToString()),
                            new Claim(ClaimTypes.Role, Roles)
                        };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "Dev Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);



                    return RedirectToAction("Index", "Trang1", new { Area = "Dev" });
                }
            }
            catch
            {
                return RedirectToAction("LoginDev", "Dev", new { Area = "Dev" });
            }
            return RedirectToAction("LoginDev", "Dev", new { Area = "Dev" });
        }

        // GET: DevController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DevController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DevController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DevController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DevController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DevController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DevController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
