using FinalProject_340.Middleware;
using FinalProject_340.Utilities;
using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;


namespace FinalProject_340.Controllers
{
    public class apiController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public apiController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                string uniqueFileName = Path.GetRandomFileName() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Save the file path in the user's session or database for future use
                // Example: HttpContext.Session.SetString("BackgroundImagePath", filePath);

                // Set the background image dynamically
                // Example: ViewBag.BackgroundImagePath = filePath;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult uploadTrack([FromForm] _n_song song)
        {
            song.setProps();

            if (!song.isValid()) return Json(false);

            return Json(song.saveTrack());
        }
        [HttpPost] 
        public IActionResult updateTrack([FromForm] Song update)
        {   
            return Json(update.updateThis());
        }
        [HttpGet]
        public IActionResult deleteTrack(string hash)
        {
            //returns true or false
            return Json(Users_Service._user.deleteTrack(hash));
        }
    }
}
