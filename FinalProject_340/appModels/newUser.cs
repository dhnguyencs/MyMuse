using System.ComponentModel.DataAnnotations;
using FinalProject_340.Utilities;
using FinalProject_340.Models;

namespace FinalProject_340.Models
{
    public class newUser : Users
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Required!")]
        public string? PASSWORD { get; set; }

        [Display(Name = "Password")]
        public string? PASSWORDCNF { get; set; }

        public bool RegisterUser()
        {
            //basic checks
            if (
                EMAIL == null || FIRST_NAME == null ||
                EMAIL == null || PASSWORD == null ||
                PASSWORDCNF == null || PASSWORD != PASSWORDCNF
                ) return false;

            /*create a SqlDBConnection object modeling the Users table in the database, 
              using the connection string found in the resource file*/
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(Properties.Resource.appData);
            //Create a new user with the required data
            Users newUser = new Users(EMAIL, FIRST_NAME, LAST_NAME, PASSWORD);
            //return the results of function insertIntoTable from the Sql object
            return newConnection.insertIntoTable(newUser);
        }

        override
        public string ToString()
        {
            return FIRST_NAME + " " + LAST_NAME + " " + EMAIL + " " + PASSWORD;
        }
    }
}
