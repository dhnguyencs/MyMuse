using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(newUser newUser)
        {
            if(newUser.PASSWORD != newUser.PASSWORDCNF || !newUser.RegisterUser())
            {
                return View();
            }
            SessionTokens newToken = new SessionTokens(newUser.UUID);
            if (newToken.registerToken())
            {
                SetCookie("sessionID", newToken.SessionID, 99);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Login");
        }
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
    }
}
