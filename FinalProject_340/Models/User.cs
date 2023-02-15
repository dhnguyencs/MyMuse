using Microsoft.Data.SqlClient;
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
            if (String.IsNullOrEmpty(cookie)) return (Users?)null;
            SessionTokens? newToken = SessionTokens.getToken(cookie);
            if (newToken == null || newToken.accountHash.IsNullOrEmpty()) return (Users?)null;
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            Users? user = newConnection.getFirstOrDefault(new Dictionary<string, string>() { { "UUID", newToken.accountHash } });
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

        public Users() { }
        public Users(string EMAIL, string PASSWORD)
        {

            String abc = new String("Hello World");

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
    }
}
