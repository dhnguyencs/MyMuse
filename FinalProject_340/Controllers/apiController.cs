using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FinalProject_340.Controllers
{
    public class apiController : Controller
    {
        public IActionResult Index()
        {
            return Json("hello world!");
        }
        [HttpPost]
        public IActionResult uploadSong([FromForm] __NewSong song)
        {
            if (!ModelState.IsValid) return Json(false);

            using (var stream = song.formFile.OpenReadStream())
            {
                //get cookie from request if any
                string? cookieValueFromReq = Request.Cookies["SessionID"];
                //get user with cookie
                Users? user = Users.getUser(cookieValueFromReq);
                //if either cookie or user is null, redirect to the login page instead
                if (cookieValueFromReq == null || user == null) return RedirectToAction("Index", "Login");

                var tagFile = TagLib.File.Create(new GenericAudioStream(song.formFile.FileName, stream));

                if (!tagFile.MimeType.ToLower().Contains("mp3") && !tagFile.MimeType.ToLower().Contains("ogg") &&
                    !tagFile.MimeType.ToLower().Contains("m4a") && !tagFile.MimeType.ToLower().Contains("m4a"))
                    return Json(false);

                string FTYPE = "";
                if (tagFile.MimeType.ToLower().Contains("mp3")) FTYPE = ".mp3";
                if (tagFile.MimeType.ToLower().Contains("ogg")) FTYPE = ".ogg";
                if (tagFile.MimeType.ToLower().Contains("m4a")) FTYPE = ".m4a";
                if (tagFile.MimeType.ToLower().Contains("m4a")) FTYPE = ".m4a";

                string newHash = (song.formFile.GetHashCode().ToString() + user.UUID).toHash();

                SaveFileAsync(song.formFile, "wwwroot/resources/" + user.UUID + "/songs", newHash + FTYPE);

                Song newSong = new Song()
                {
                    USR_UUID = user.UUID,
                    title = song.title,
                    artist = song.artist,
                    plays = 0,
                    songHash = newHash,
                    songLength = (int)tagFile.Properties.Duration.TotalSeconds,
                    fav = 0,
                    type = FTYPE
                };
                return Json(user.AddSong(newSong));
            }
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folderPath, string fileName)
        {
            // If directory does not exist, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // Combine the folder path and the file name to get the full file path
            var filePath = Path.Combine(folderPath, fileName);

            // Open a FileStream object to write the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the contents of the uploaded file to the FileStream
                await file.CopyToAsync(stream);
            }

            // Return the file name
            return fileName;
        }
    }
}
