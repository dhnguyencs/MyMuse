using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Models
{
    public class Song
    {
        [FromForm(Name = "title")]
        public String  title        { get; set; }
        [FromForm(Name = "artist")]
        public String  artist       { get; set; }
        [FromForm(Name = "album")]
        public String  album        { get; set; }
        [FromForm(Name = "albumArt")]
        public String  albumArt     { get; set; }
        [FromForm(Name = "songHash")]
        public String  songHash     { get; set; }
        public String  USR_UUID     { get; set; }
        public String  type         { get; set; }
        [FromForm(Name = "fav")]
        public int     fav          { get; set; }
        public int     songLength   { get; set; }
        public int     plays        { get; set; }

        public String formatTime()
        {
            int minutes = songLength / 60;
            int seconds = songLength % 60;
            StringBuilder str_sec = new StringBuilder();
            if (seconds == 0)
                str_sec.Append("00");
            else if(seconds < 10)
                str_sec.Append("0" + seconds);
            else
                str_sec.Append(seconds);
            return new String(minutes + ":" + str_sec.ToString());
        }
        public bool favourited() { return fav == 0 ? false : true; }
        public bool checkPath(int x, int y)
        {
            if (!File.Exists("wwwroot/resources/" + USR_UUID + "/art/" + songHash + x + "x" + y + ".jpg"))
                return false;
            return true;
        }

    }
}
