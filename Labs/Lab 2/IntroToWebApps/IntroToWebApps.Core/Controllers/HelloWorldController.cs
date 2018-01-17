using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IntroToWebApps.Core.Models;

namespace IntroToWebApps.Core.Controllers.Core.Controllers
{
    [Route("[controller]"), Route("/")]
    public class HelloWorldController : Controller
    {
        [HttpGet]
        [Route("[action]"), Route("")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(Person person)
        {
            return View(person);
        }
    }
}
