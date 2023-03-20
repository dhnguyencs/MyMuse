using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            //get cookie
            string ? cookie = Request.Cookies["sessionID"];
            //authenticate user with cookie
            Users? user = Users.getUser(cookie);
            //check if either user is null or cookie is null
            if (user != null && !String.IsNullOrEmpty(user.UUID))
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login info)
        {
            //if login info state is invalid, return to the login page
            if (!ModelState.IsValid || String.IsNullOrEmpty(info.EMAIL) || String.IsNullOrEmpty(info.PASSWORD)) return RedirectToAction("Index", "Login");

            //create a new sq connection to the users table
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            //hash the provided username + password, then attempt to retrieve user with the hash
            Users? user = newConnection.getFirstOrDefault(new Dictionary<string, string>()
            {
                {"UUID",  (info.EMAIL + info.PASSWORD).toHash()}
            });
            //if user is null redirect to login page
            if(user == null) return RedirectToAction("Index", "Login");

            //otherwise, create a new token with the user's uuid 
            SessionTokens newToken = new SessionTokens(user.UUID);

            //then register the token
            if (newToken.registerToken())
            {
                //if success, set the cookie so we can retrieve the login token again later
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
            //if the passwords dont match or unable to register user into the database, return to the account creation page
            if(newUser.PASSWORD != newUser.PASSWORDCNF || !newUser.RegisterUser())
            {
                return View();
            }
            //else, redirect to the login action
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
