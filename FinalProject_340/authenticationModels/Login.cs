using System.ComponentModel.DataAnnotations;
using FinalProject_340.Models;

namespace FinalProject_340.Models
{
    public class Login
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Required!")]
        public string? EMAIL { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Required!")]
        public string? PASSWORD { get; set; }
    }
}
