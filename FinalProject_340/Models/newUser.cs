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
        public newUser(string firstName, string lastName, string email, string password)
        {
            this.FIRST_NAME = firstName;
            this.LAST_NAME = lastName;
            this.EMAIL = email;
            this.UUID  = (email + password).toHash();

            register();
        }
        public bool register()
        {
            return true;
        }

    }
}
