using FinalProject_340.Middleware;
using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using FinalProject_340.Utilities;

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
        public IActionResult uploadBackground(IFormFile file)
        {
            FileSys.SaveFile(file, "wwwroot/resources/" + Users_Service._user.UUID, "background.jpg");
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
