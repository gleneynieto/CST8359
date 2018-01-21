using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab3.Controllers
{
    public class Home : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("Razor");
        }

        public IActionResult Razor()
        {
            return View();
        }

        public IActionResult CreatePerson()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DisplayPerson()
        {
            HttpContext.Session.SetString("firstName", Request.Form["firstName"]);
            HttpContext.Session.SetString("lastName", Request.Form["lastName"]);
            HttpContext.Session.SetString("age", Request.Form["age"]);
            HttpContext.Session.SetString("emailAddress", Request.Form["emailAddress"]);
            HttpContext.Session.SetString("dob", Request.Form["dob"]);
            HttpContext.Session.SetString("password", Request.Form["password"]);
            HttpContext.Session.SetString("description", Request.Form["description"]);

            return View();
        }
    }
}
