using FinalProject_340.Middleware;
using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace FinalProject_340.Controllers
{
    public class apiController : Controller
    {
        [HttpPost]
        public IActionResult uploadTrack([FromForm] _n_song song)
        {
            using (var stream = song.formFile.OpenReadStream())
            {
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

                string newHash = (song.formFile.GetHashCode().ToString() + Users_Service._user.UUID).toHash();

                SaveFileAsync(song.formFile, "wwwroot/resources/" + Users_Service._user.UUID + "/songs", newHash + FTYPE);

                if(song.albumArt != null) saveImages(song.albumArt, Users_Service._user.UUID, newHash);

                return Json(Users_Service._user.AddSong(
                    new Song()
                        {
                            USR_UUID    = Users_Service._user.UUID                              ,
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
        [HttpPost] 
        public IActionResult updateTrack([FromForm] Song update)
        {
            //retrieve the track to be updated from the database
            Song song = Users_Service._user.getTrack(update.songHash);

            if(update.plays == 921873)
            {
                song.plays++;
                return Json(Users_Service._user.updateTrack(song));
            }

            //update operations
            song.title = update.title;
            song.artist = update.artist;
            song.album = update.album;

            //return the results of pushing the update to the database
            return Json(Users_Service._user.updateTrack(song));
        }
        [HttpGet]
        public IActionResult deleteTrack(string hash)
        {
            //returns true or false
            return Json(Users_Service._user.deleteTrack(hash));
        }
        public void saveImages(IFormFile file, string UUID, string HASH)
        {
            if (!Directory.Exists("wwwroot/resources/" + UUID + "/art/")) 
                Directory.CreateDirectory("wwwroot/resources/" + UUID + "/art/");
            resizeImage(file, "wwwroot/resources/" + UUID + "/art/" + HASH + "30x30.jpg", 30, 30);
            resizeImage(file, "wwwroot/resources/" + UUID + "/art/" + HASH + "100x100.jpg", 100, 100);
            resizeImage(file, "wwwroot/resources/" + UUID + "/art/" + HASH + "500x500.jpg", 500, 500);
        }
        public void resizeImage(IFormFile inputFile, string outputFilePath, int newWidth, int newHeight)
        {
            using (var image = new Bitmap(inputFile.OpenReadStream()))
            {
                var resizedImage = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                resizedImage.Save(outputFilePath, ImageFormat.Jpeg);
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
