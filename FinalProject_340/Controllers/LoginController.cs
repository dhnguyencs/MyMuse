using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            string ? cookie = Request.Cookies["sessionID"];
            Users? user = Users.getUser(cookie);
            if (user != null && !String.IsNullOrEmpty(user.UUID))
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login info)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Login");

            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            Users? user = newConnection.getFirstOrDefault(new Dictionary<string, string>()
            {
                {"UUID",  (info.EMAIL + info.PASSWORD).toHash()}
            });
            if(user == null) return RedirectToAction("Index", "Login");

            SessionTokens newToken = new SessionTokens(user.UUID);
            if (newToken.registerToken())
            {
                SetCookie("sessionID", newToken.SessionID, 99);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Login");
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
            return Login(new Login()
            {
                EMAIL = newUser.EMAIL,
                PASSWORD = newUser.PASSWORD
            });
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
