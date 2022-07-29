using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mproject.Data;
using mproject.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using mproject.Utility;
namespace mproject.Controllers
{
    public class ImageController : Controller
    {

        private readonly AppDbContext context;
        public ImageController(AppDbContext Context)
        {
            this.context = Context;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var imageName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var uploadedby = HttpContext.User.Identity.Name;
                var imageModel = new ImageonDBModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    ImageName = imageName,
                    UploadedBy = uploadedby,
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    imageModel.Data = dataStream.ToArray();
                }
                context.ImageOnDatabase.Add(imageModel);
                context.SaveChanges();
            }
            return RedirectToAction("Upload", "Home");
        }
        public async Task<IActionResult> ShowImage(imageconvert img)
        {
            return View();
        }
    }
}
