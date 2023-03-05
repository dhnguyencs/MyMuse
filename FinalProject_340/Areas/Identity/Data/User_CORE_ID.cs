using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_340.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject_340.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User_CORE_ID class
public class User_CORE_ID : IdentityUser
{
    //public string? UUID { get; set; }

    //[Display(Name = "First Name")]
    //[Required(ErrorMessage = "First Name Cannot Be Blank!")]
    //public string? FIRST_NAME { get; set; }

    //[Display(Name = "Last Name")]
    //[Required(ErrorMessage = "Last Name Cannot Be Blank!")]
    //public string? LAST_NAME { get; set; }

    //[Display(Name = "Email")]
    //[Required(ErrorMessage = "Email Required!")]
    //public string? EMAIL { get; set; }

    //private Dictionary<string, string> _listSet = new Dictionary<string, string>();

    //public User_CORE_ID() { }
    //public User_CORE_ID(string EMAIL, string PASSWORD)
    //{
    //    UUID = (EMAIL + PASSWORD).toHash();
    //}
    //public User_CORE_ID(string _EMAIL, string FIRSTNAME, string LASTNAME, string _PASSWORD)
    //{
    //    FIRST_NAME = FIRSTNAME;
    //    LAST_NAME = LASTNAME;
    //    EMAIL = _EMAIL;
    //    UUID = (_EMAIL + _PASSWORD).toHash();
    //}
    //public IDictionary<string, string> getList()
    //{
    //    return _listSet;
    //}
    //public bool AddSong(Song song)
    //{
    //    SqlDBConnection<Song> sqlDBConnection = new SqlDBConnection<Song>(FinalProject_340.Properties.Resource.appData);
    //    if (sqlDBConnection.insertIntoTable(song)) return true;
    //    return false;
    //}
    //List<Song> getAllSongs()
    //{
    //    SqlDBConnection<Song> sqlDBConnection = new SqlDBConnection<Song>(FinalProject_340.Properties.Resource.appData);
    //    List<Song> songs = sqlDBConnection.getList(new Dictionary<string, string>() {
    //            {
    //                "USR_UUID", this.UUID
    //            }
    //        }, 9999);
    //    return songs;
    //}
}
