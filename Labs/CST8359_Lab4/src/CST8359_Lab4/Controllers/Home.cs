using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CST8359_Lab4.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CST8359_Lab4.Controllers
{
    public class Home : Controller
    {
        private MovieContext _movieContext;

        public Home(MovieContext context)
        {
            _movieContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_movieContext.Movies.ToList());
        }

        public IActionResult CreateMovie(Movie movie)
        {
            _movieContext.Movies.Add(movie);
            _movieContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult EditMovie(int id)
        {
            var movieToUpdate = (from m in _movieContext.Movies where m.MovieId == id select m).FirstOrDefault();

            return View(movieToUpdate);
        }
    }
}
