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
            var indexViewList = new List<IndexViewModel>(); 

            var blogPost = _assignment1DataContext.BlogPosts.ToList();

            foreach(var b in blogPost)
            {
                var indexViewModel = new IndexViewModel();

                indexViewModel.BlogPost = b;

                indexViewModel.User = (from u in _assignment1DataContext.Users where u.UserId == b.UserId select u).FirstOrDefault();

                indexViewList.Add(indexViewModel);
            }

            indexViewList.Reverse();

            return View(indexViewList);
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
                HttpContext.Session.SetString("_firstName", userToValidate.FirstName);

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
            var displayFullBlogPostViewModel = new DisplayFullBlogPostViewModel();

            var blogPostToShow = (from b in _assignment1DataContext.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();
            displayFullBlogPostViewModel.BlogPost = blogPostToShow;

            var user = (from u in _assignment1DataContext.Users where blogPostToShow.UserId == u.UserId select u).FirstOrDefault();
            displayFullBlogPostViewModel.User = user;

            var comments = (from c in _assignment1DataContext.Comments where c.BlogPostId == id select c).ToList();
            displayFullBlogPostViewModel.Comments = comments;

            return View(displayFullBlogPostViewModel);
        }

        public IActionResult PostComment(Comment comment)
        {
            _assignment1DataContext.Comments.Add(comment);
            _assignment1DataContext.SaveChanges();

            return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId });
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

        public IActionResult EditBlogPost(int id)
        {
            var blogPostToUpdate = (from b in _assignment1DataContext.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();

            return View(blogPostToUpdate);
        }

        public IActionResult ModifyBlogPost(BlogPost blogPost)
        {
            var blogPostToUpdate = (from b in _assignment1DataContext.BlogPosts where b.BlogPostId == blogPost.BlogPostId select b).FirstOrDefault();
            blogPostToUpdate.Title = blogPost.Title;
            blogPostToUpdate.Posted = blogPost.Posted;
            blogPostToUpdate.Content = blogPost.Content;

            _assignment1DataContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
