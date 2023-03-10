using System.Text;

namespace FinalProject_340.Models
{
    public class Song
    {
        public int     plays        { get; set; }
        public String  title        { get; set; }
        public String  artist       { get; set; }
        public String  album        { get; set; }
        public String  albumArt     { get; set; }
        public String  songHash     { get; set; }
        public String  USR_UUID     { get; set; }
        public String  type         { get; set; }
        public int     fav          { get; set; }
        public int     songLength   { get; set; }

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
        public bool favourited()
        {
            return fav == 0 ? false : true;
        }

    }
}
