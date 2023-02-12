using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Crea
    }
}
