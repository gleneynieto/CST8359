using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment1.Controllers
{
    public class Home : Controller
    {
        private Assignment1DataContext _assignment1DataContext;

        public Home(Assignment1DataContext context)
        {
            _assignment1DataContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_assignment1DataContext.BlogPosts.ToList());
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterUser(User user)
        {

            if (ModelState.IsValid)
            {
                _assignment1DataContext.Users.Add(user);
                _assignment1DataContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ValidateLogin(User user)
        {
            var userToValidate = (from u in _assignment1DataContext.Users where u.EmailAddress == user.EmailAddress && u.Password == user.Password select u).FirstOrDefault();

            if (userToValidate != null)
            {
                //Save User Id in session state
                HttpContext.Session.SetInt32("_userId", userToValidate.UserId);
                HttpContext.Session.SetInt32("_roleId", userToValidate.RoleId);

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.isLoggedIn = false;

                return RedirectToAction("Login");
            }
        }

        public IActionResult DisplayFullBlogPost(int id)
        {
            var blogPostToShow = (from b in _assignment1DataContext.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();

            return View(blogPostToShow);
        }

        public IActionResult PostComment(Comment comment)
        {
            _assignment1DataContext.Comments.Add(comment);
            _assignment1DataContext.SaveChanges();

            return View();
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }

        public IActionResult CreateBlogPost(BlogPost blogPost)
        {
            _assignment1DataContext.BlogPosts.Add(blogPost);
            _assignment1DataContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
