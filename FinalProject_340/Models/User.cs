﻿using Microsoft.Data.SqlClient;
using FinalProject_340;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject_340.Models
{
    public class Users
    {
        public static Users? getUser(String ? cookie)
        {
            //if cookie is null, return a null user
            if (String.IsNullOrEmpty(cookie)) return (Users?)null;
            //retrieve session token from cookie
            SessionTokens? newToken = SessionTokens.getToken(cookie);
            //if the token is null or the account UUID associated with token is null, return a null user
            if (newToken == null || newToken.accountHash.IsNullOrEmpty()) return (Users?)null;
            //create a new sql db connection targeting the user table
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            //retrieve the user using the account UUID in the token
            Users? user = newConnection.getFirstOrDefault(new Dictionary<string, string>() { { "UUID", newToken.accountHash } });
            //return either the user or null user
            return user != null ? user : (Users?)null;
        }

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

        SqlDBConnection<Song> sqlDB_tracks = new SqlDBConnection<Song>(FinalProject_340.Properties.Resource.appData);

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
                    "USR_UUID", this.UUID
                }
            });
            return true;
        }
        public List<Song> getAllTracks(int order)
        {
            List<Song> songs = sqlDB_tracks.getList(new Dictionary<string, string>() {
                {
                    "USR_UUID", this.UUID
                }
            }, 9999);
            if(order == 1)
                songs.Sort((x, y) => x.title.ToLower().CompareTo(y.title.ToLower()));
            if (order == 2)
                songs.Sort((x, y) => x.songLength.CompareTo(y.songLength));
            if (order == 3)
                songs.Sort((x, y) => x.artist.ToLower().CompareTo(y.artist.ToLower()));
            if (order == 4)
                songs.Sort((x, y) => x.album.ToLower().CompareTo(y.album.ToLower()));
            if (order == 5)
                songs.Sort((x, y) => x.plays.CompareTo(y.plays));
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
                    "USR_UUID", this.UUID
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
                    "USR_UUID", this.UUID
                }
            });
        }
    }
}
