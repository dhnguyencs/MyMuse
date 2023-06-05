using FinalProject_340.Middleware;
using FinalProject_340.Utilities;
using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FinalProject_340.Models
{
    public class _n_song
    {
        [FromForm(Name = "formFile")]
        public IFormFile formFile { get; set; }

        [FromForm(Name = "albumArt")]
        public IFormFile albumArt { get; set; }

        [FromForm(Name = "title")]
        public string title { get; set; }

        [FromForm(Name = "album")]
        public string album { get; set; }

        [FromForm(Name = "artist")]
        public string artist { get; set; }

        public int duration { get; set; }

        private string MiemeType = "";
        private string FTYPE = "";
        public void setProps()
        {
            using (Stream stream = formFile.OpenReadStream())
            {
                TagLib.File tagFile = TagLib.File.Create(new GenericAudioStream(formFile.FileName, stream));
                MiemeType = tagFile.MimeType.ToLower();
                if (MiemeType.Contains("mp3")) FTYPE = ".mp3";
                if (MiemeType.Contains("ogg")) FTYPE = ".ogg";
                if (MiemeType.Contains("m4a")) FTYPE = ".m4a";
                if (MiemeType.Contains("wav")) FTYPE = ".wav";
                duration = (int)tagFile.Properties.Duration.TotalSeconds;
            }
        }

        public bool isValid()
        {
            if (
                !MiemeType.Contains("mp3") &&
                !MiemeType.Contains("ogg") &&
                !MiemeType.Contains("m4a") &&
                !MiemeType.Contains("wav")
            ) return false;
            if (string.IsNullOrEmpty(FTYPE)) return false;
            return true;
        }

        public bool saveTrack()
        {
            string newHash = (formFile.GetHashCode().ToString() + Users_Service._user.UUID).toHash();

            FileSys.SaveFileAsync(formFile, "wwwroot/resources/" + Users_Service._user.UUID + "/songs", newHash + FTYPE);

            if (albumArt != null) FileSys.saveAlbumArt(albumArt, Users_Service._user.UUID, newHash);
            return Users_Service._user.AddSong(
                new Song()
                    {
                        USR_UUID    = Users_Service._user.UUID  ,
                        title       = title                     ,
                        artist      = artist                    ,
                        album       = album                     ,
                        songHash    = newHash                   ,
                        songLength  = duration                  ,
                        plays       = 0                         ,
                        fav         = 0                         ,
                        type        = FTYPE                                            
                    }
            );
        }

    }
}
