using System.ComponentModel.DataAnnotations;

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
                this.EMAIL          == null || this.FIRST_NAME == null || 
                this.EMAIL          == null || this.PASSWORD   == null ||
                this.PASSWORDCNF    == null || this.PASSWORD   != this.PASSWORDCNF
                ) return false;
            
            /*create a SqlDBConnection object modeling the Users table in the database, 
              using the connection string found in the resource file*/
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            //Create a new user with the required data
            Users newUser = new Users(this.EMAIL, this.FIRST_NAME, this.LAST_NAME, this.PASSWORD);
            //return the results of function insertIntoTable from the Sql object
            return newConnection.insertIntoTable(newUser);
        }

        override
        public String ToString()
        {
            return this.FIRST_NAME + " " + this.LAST_NAME + " " + this.EMAIL + " " + this.PASSWORD;
        }
    }
}
