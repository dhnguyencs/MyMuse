using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FinalProject_340.Controllers
{
    public class apiController : Controller
    {
        [HttpPost]
        public IActionResult uploadSong([FromForm] __NewSong song)
        {
            //if (!ModelState.IsValid) return Json(false);

            using (var stream = song.formFile.OpenReadStream())
            {
                //get cookie from request if any
                string? cookieValueFromReq = Request.Cookies["SessionID"];
                //get user with cookie
                Users? user = Users.getUser(cookieValueFromReq);
                //if either cookie or user is null, redirect to the login page instead
                if (cookieValueFromReq == null || user == null) return RedirectToAction("Index", "Login");
                var tagFile = TagLib.File.Create(new GenericAudioStream(song.formFile.FileName, stream));
                string MiemeType = tagFile.MimeType.ToLower();
                if (
                        !MiemeType.Contains("mp3") && 
                        !MiemeType.Contains("ogg") &&
                        !MiemeType.Contains("m4a") && 
                        !MiemeType.Contains("wav")
                    ) return Json(false);

                string FTYPE = "";
                if (MiemeType.Contains("mp3")) FTYPE = ".mp3";
                if (MiemeType.Contains("ogg")) FTYPE = ".ogg";
                if (MiemeType.Contains("m4a")) FTYPE = ".m4a";
                if (MiemeType.Contains("wav")) FTYPE = ".wav";

                string newHash = (song.formFile.GetHashCode().ToString() + user.UUID).toHash();

                SaveFileAsync(song.formFile, "wwwroot/resources/" + user.UUID + "/songs", newHash + FTYPE);

                return Json(user.AddSong(
                    new Song()
                        {
                            USR_UUID    = user.UUID                                             ,
                            title       = song.title                                            ,
                            artist      = song.artist                                           ,
                            album       = song.album                                            ,
                            songHash    = newHash                                               ,
                            songLength  = (int)tagFile.Properties.Duration.TotalSeconds         ,
                            plays       = 0                                                     ,
                            fav         = 0                                                     ,
                            type        = FTYPE
                        }
                ));
            }
        }
        public async Task<int> SaveFileAsync(IFormFile file, string folderPath, string fileName)
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
            return 0;
        }
    }
}
