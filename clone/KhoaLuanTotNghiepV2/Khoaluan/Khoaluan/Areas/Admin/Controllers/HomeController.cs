﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DuAnGame.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("admin.html", Name = "AdminIndex")]
    public class TrangController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
