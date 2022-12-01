using AspNetCoreHero.ToastNotification.Abstractions;
using Khoaluan.Models;
using Microsoft.AspNet.SignalR;
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
        [Authorize]
        //[Route("chat.html", Name = "Chat")]
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            var currentUser = _unitOfWork.UserRepository.GetById(int.Parse(taikhoanID));
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.HoTen;
                ViewBag.Id = currentUser.Id;
            }
            var messages = _unitOfWork.MessageRepository.getMess();
            return View(messages);
        }
        [Authorize]
        public IActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                var sender = _unitOfWork.UserRepository.GetById(int.Parse(taikhoanID));
                message.UserID = sender.Id;
                _unitOfWork.MessageRepository.Create(message);
                _unitOfWork.SaveChange();
                return Ok();
            }
            return View("Index");
        }
    }
}
