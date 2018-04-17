using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Lab5.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab5.Controllers
{
    public class HomeController : Controller
    {
        private PhotoDataContext _context;

        public HomeController(PhotoDataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_context.Photos.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(ICollection<IFormFile> files)
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
                    _context.Photos.Add(photo);
                    _context.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFile(int id)
        {
            var photoToDelete = (from p in _context.Photos where p.PhotoId == id select p).FirstOrDefault();

            _context.Remove(photoToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
