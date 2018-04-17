using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Assignment2.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment2.Controllers
{
    public class Home : Controller
    {
        private Assignment2DataContext _assignment2DataContext;

        public Home(Assignment2DataContext context)
        {
            _assignment2DataContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var indexViewList = new List<IndexViewModel>(); 

            var blogPost = _assignment2DataContext.BlogPosts.ToList();

            foreach(var b in blogPost)
            {
                var indexViewModel = new IndexViewModel();

                indexViewModel.BlogPost = b;

                indexViewModel.User = (from u in _assignment2DataContext.Users where u.UserId == b.UserId select u).FirstOrDefault();

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
                _assignment2DataContext.Users.Add(user);
                _assignment2DataContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ValidateLogin(User user)
        {
            var userToValidate = (from u in _assignment2DataContext.Users where u.EmailAddress == user.EmailAddress && u.Password == user.Password select u).FirstOrDefault();

            if (userToValidate != null)
            {
                //Save User Id in session state
                HttpContext.Session.SetInt32("_userId", userToValidate.UserId);
                HttpContext.Session.SetInt32("_roleId", userToValidate.RoleId);
                HttpContext.Session.SetString("_firstName", userToValidate.FirstName);
                HttpContext.Session.SetString("_lastName", userToValidate.LastName);

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

            var blogPostToShow = (from b in _assignment2DataContext.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();
            displayFullBlogPostViewModel.BlogPost = blogPostToShow;

            var user = (from u in _assignment2DataContext.Users where blogPostToShow.UserId == u.UserId select u).FirstOrDefault();
            displayFullBlogPostViewModel.User = user;

            var comments = (from c in _assignment2DataContext.Comments where c.BlogPostId == id select c).ToList();
            displayFullBlogPostViewModel.Comments = comments;

            var photos = (from p in _assignment2DataContext.Photos where p.BlogPostId == id select p).ToList();
            displayFullBlogPostViewModel.Photos = photos;

            return View(displayFullBlogPostViewModel);
        }

        public IActionResult PostComment(Comment comment)
        {
            var punctuation = comment.Content.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = comment.Content.Split().Select(x => x.Trim(punctuation));

            foreach(var word in words)
            {
                var badWord = (from b in _assignment2DataContext.BadWords where word.Equals(b.Word, StringComparison.OrdinalIgnoreCase) select b).FirstOrDefault();

                if(badWord != null)
                {
                    comment.Content = comment.Content.Replace(badWord.Word, "*****");
                }
            }

            _assignment2DataContext.Comments.Add(comment);
            _assignment2DataContext.SaveChanges();

            return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId });
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }

        public IActionResult CreateBlogPost(BlogPost blogPost)
        {
            _assignment2DataContext.BlogPosts.Add(blogPost);
            _assignment2DataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult EditBlogPost(int id)
        {
            var editBlogPostViewModel = new EditBlogPostViewModel();

            editBlogPostViewModel.BlogPost = (from b in _assignment2DataContext.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();

            var photosToUpdate = (from p in _assignment2DataContext.Photos where editBlogPostViewModel.BlogPost.BlogPostId == p.BlogPostId select p).ToList();

            editBlogPostViewModel.Photos = photosToUpdate;

            return View(editBlogPostViewModel);
        }

        public IActionResult ModifyBlogPost(BlogPost blogPost)
        {
            var blogPostToUpdate = (from b in _assignment2DataContext.BlogPosts where b.BlogPostId == blogPost.BlogPostId select b).FirstOrDefault();
            blogPostToUpdate.Title = blogPost.Title;
            blogPostToUpdate.Posted = blogPost.Posted;
            blogPostToUpdate.Content = blogPost.Content;

            _assignment2DataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult EditProfile(int id)
        {
            var userToUpdate = (from u in _assignment2DataContext.Users where u.UserId == id select u).FirstOrDefault();

            return View(userToUpdate);
        }

        public IActionResult ModifyProfile(User user)
        {
            var id = Convert.ToInt32(Request.Form["UserId"]);

            var userToUpdate = (from u in _assignment2DataContext.Users where u.UserId == id select u).FirstOrDefault();
            userToUpdate.RoleId = user.RoleId;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.EmailAddress = user.EmailAddress;
            userToUpdate.Password = user.Password;
            userToUpdate.DateOfBirth = user.DateOfBirth;
            userToUpdate.Address = user.Address;
            userToUpdate.City = user.City;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.Country = user.Country;

            _assignment2DataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteBlogPost(int id)
        {
            /*
            var photosToDelete = (from p in _assignment2DataContext.Photos where p.BlogPostId == id select p).ToList();

            if(photosToDelete != null)
            {
                foreach (var photo in photosToDelete)
                {
                    _assignment2DataContext.Remove(photo);
                }
            }

            var commentsToDelete = (from c in _assignment2DataContext.Comments where c.BlogPostId == id select c).ToList();

            if (commentsToDelete != null)
            {
                foreach (var comment in commentsToDelete)
                {
                    _assignment2DataContext.Remove(comment);
                }
            }
            */
            var blogPostToDelete = (from b in _assignment2DataContext.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();

            _assignment2DataContext.Remove(blogPostToDelete);
            _assignment2DataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(ICollection<IFormFile> files, BlogPost blogPost)
        {

            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=cst8359;AccountKey=ecMPpNU6vimZKMDTJG4seALrY7Kq7UJYjgl0/yLanXn857C8xtUJ2sF4ciB6wy9gg+e/YeYbRTaly2DVOxWhXQ==");

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("petersphotostorage");
            await container.CreateIfNotExistsAsync();

            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);

            foreach (var file in files)
            {
                try
                {
                    var blockBlob = container.GetBlockBlobReference(file.FileName);
                    if (await blockBlob.ExistsAsync())
                        await blockBlob.DeleteAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        memoryStream.Position = 0;

                        await blockBlob.UploadFromStreamAsync(memoryStream);
                    }

                    var photo = new Photo();
                    photo.Url = blockBlob.Uri.AbsoluteUri;
                    photo.FileName = file.FileName;
                    photo.BlogPostId = blogPost.BlogPostId;
                    
                    _assignment2DataContext.Photos.Add(photo);
                    _assignment2DataContext.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("EditBlogPost", new { id = blogPost.BlogPostId } );
        }

        public IActionResult DeletePhoto(int id)
        {
            var photoToDelete = (from p in _assignment2DataContext.Photos where p.PhotoId == id select p).FirstOrDefault();

            _assignment2DataContext.Remove(photoToDelete);
            _assignment2DataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ViewBadWords()
        {
            return View(_assignment2DataContext.BadWords.ToList());
        }

        public IActionResult AddBadWord(BadWord badWord)
        {
            _assignment2DataContext.BadWords.Add(badWord);
            _assignment2DataContext.SaveChanges();

            return RedirectToAction("ViewBadWords");
        }
    }
}
