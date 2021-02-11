using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Controllers
{
    public class Account : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
