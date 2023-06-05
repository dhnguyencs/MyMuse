using Microsoft.Data.SqlClient;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using FinalProject_340.Utilities;
using Azure.Core;
using FinalProject_340.Models;
using Microsoft.AspNetCore.Html;

namespace FinalProject_340.Models
{
    public class Users
    {
        public string? UUID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name Cannot Be Blank!")]
        public string? FIRST_NAME { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name Cannot Be Blank!")]
        public string? LAST_NAME { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Required!")]
        public string? EMAIL { get; set; }

        private Dictionary<string, string> _listSet = new Dictionary<string, string>();

        SqlDBConnection<Song> sqlDB_tracks = new SqlDBConnection<Song>(Properties.Resource.appData);

        public Users() { }
        public Users(string EMAIL, string PASSWORD)
        {
            UUID = (EMAIL + PASSWORD).toHash();
        }
        public Users(string _EMAIL, string FIRSTNAME, string LASTNAME, string _PASSWORD)
        {
            FIRST_NAME  = FIRSTNAME;
            LAST_NAME   = LASTNAME;
            EMAIL       = _EMAIL;
            UUID        = (_EMAIL + _PASSWORD).toHash();
        }
        public IDictionary<string, string> getList()
        {
            return _listSet;
        }
        public bool AddSong(Song song)
        {
            return sqlDB_tracks.insertIntoTable(song);
        }
        public bool updateTrack(Song song)
        {
            sqlDB_tracks.update(song, new Dictionary<string, string>()
            {
                {
                    "SongHash", song.songHash
                },
                {
                    "USR_UUID", UUID
                }
            });
            return true;
        }
        public List<Song> getAllTracks(int order)
        {
            List<Song> songs = sqlDB_tracks.getList(new Dictionary<string, string>() {
                {
                    "USR_UUID", UUID
                }
            }, 9999);
            if (order == 1) songs.Sort((x, y) => x.title.ToLower().CompareTo(y.title.ToLower()));
            if (order == 2) songs.Sort((x, y) => x.songLength.CompareTo(y.songLength));
            if (order == 3) songs.Sort((x, y) => x.artist.ToLower().CompareTo(y.artist.ToLower()));
            if (order == 4) songs.Sort((x, y) => x.album.ToLower().CompareTo(y.album.ToLower()));
            if (order == 5) songs.Sort((x, y) => x.plays.CompareTo(y.plays));
            return songs;
        }
        public Song getTrack(string hash)
        {
            return sqlDB_tracks.getFirstOrDefault(new Dictionary<string, string>()
            {
                {
                    "songHash", hash
                },
                {
                    "USR_UUID", UUID
                }
            });
        }
        public bool deleteTrack(string hash)
        {
            return sqlDB_tracks.delete(new Dictionary<string, string>()
            {
                {
                    "songHash", hash
                },
                {
                    "USR_UUID", UUID
                }
            });
        }
        public HtmlString getBackGround()
        {
            if(!File.Exists("wwwroot/resources/" + UUID + "/background.jpg")){
                return new HtmlString("background: -webkit-linear-gradient(rgba(144, 238, 144, 1), rgba(60, 100, 60, 0.8)), url(\"../images/hd-wallpaper-3833973_640.jpg\");\r\n        background: linear-gradient(rgba(144, 238, 144, 1), rgba(60, 100, 60, 0.8)), url(\"../images/hd-wallpaper-3833973_640.jpg\");");
            }
            return new HtmlString("background: url(" + "/resources/" + UUID + "/background.jpg);");
        }
    }
}
