using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DuAnGame.Areas.Admin.Controllers
{
    [Authorize(Roles = "Dev")]
    [Area("Dev")]
    [Route("dev.html", Name = "DevIndex")]
    public class Trang1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
