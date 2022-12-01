using AspNetCoreHero.ToastNotification.Abstractions;
using Khoaluan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Khoaluan.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notyfService { get; }
        public ChatController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _notyfService = notyfService;
        }
        [Route("chat.html", Name = "Chat")]
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            var currentUser = _unitOfWork.UserRepository.GetById(int.Parse(taikhoanID));
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.HoTen;
                ViewBag.Id = currentUser.Id;
            }
            var messages = _unitOfWork.LibraryRepository.GetAll();
            return View(messages);
        }

        //public async Task<IActionResult> Create(Message message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        message.UserName = User.Identity.Name;
        //        var sender = await _userManager.GetUserAsync(User);
        //        message.UserID = sender.Id;
        //        await _context.Messages.AddAsync(message);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return View("Index");
        //}
    }
}
