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
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            if (this.EMAIL == null || this.FIRST_NAME == null || this.EMAIL == null || this.PASSWORD == null) return false;
            Users newUser = new Users(this.EMAIL, this.FIRST_NAME, this.LAST_NAME, this.PASSWORD);
            this.UUID = newUser.UUID;
            return newConnection.insertIntoTable(newUser);
        }

        override
        public String ToString()
        {
            return this.FIRST_NAME + " " + this.LAST_NAME + " " + this.EMAIL + " " + this.PASSWORD;
        }
    }
}
