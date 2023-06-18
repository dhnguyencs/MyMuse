using System.Text;
using FinalProject_340.Middleware;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalProject_340.Models
{
    public class Song
    {
        [FromForm(Name = "title")]
        public string title { get; set; }
        [FromForm(Name = "artist")]
        public string artist { get; set; }
        [FromForm(Name = "album")]
        public string album { get; set; }
        [FromForm(Name = "albumArt")]
        public string albumArt { get; set; }
        [FromForm(Name = "songHash")]
        public string songHash { get; set; }
        public string USR_UUID { get; set; }
        public string type { get; set; }
        [FromForm(Name = "fav")]
        public int fav { get; set; }
        public int songLength { get; set; }
        public int plays { get; set; }

        public string formatTime()
        {
            int minutes = songLength / 60;
            int seconds = songLength % 60;
            StringBuilder str_sec = new StringBuilder();
            if (seconds == 0)
                str_sec.Append("00");
            else if (seconds < 10)
                str_sec.Append("0" + seconds);
            else
                str_sec.Append(seconds);
            return new string(minutes + ":" + str_sec.ToString());
        }
        public bool checkPath(int x, int y)
        {
            if (!File.Exists("wwwroot/resources/" + USR_UUID + "/art/" + songHash + x + "x" + y + ".jpg"))
                return false;
            return true;
        }
        public bool updateThis()
        {
            //retrieve the track to be updated from the database
            Song song = Users_Service._user.getTrack(songHash);

            if (plays == 921873)
            {
                song.plays++;
                return Users_Service._user.updateTrack(song);
            }

            //update operations
            song.title = title;
            song.artist = artist;
            song.album = album;

            //return the results of pushing the update to the database
            return Users_Service._user.updateTrack(song);
        }
    }
}
